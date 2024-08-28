using _750HrsTracker.Enums;
using _750HrsTracker.Filters;
using _750HrsTracker.Helpers;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Models.SubscriptionModels;
using _750HrsTracker.Repositories.Interfaces;
using _750HrsTracker.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Stripe;
using System.Text.Json;

namespace _750HrsTracker.Services.Implementations
{
    public class WebhookNotificationService : IWebhookNotificationService
    {
        private readonly IWebhookNotificationRepository _webhookNotificationRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly INotificationService _notificationService;
        private readonly IUriService _uriService;
        private readonly ITeamSubscriptionRepository _teamSubscriptionRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly AppSettings _appSettings;
        private readonly Bugsnag.IClient _bugsnag;
        public WebhookNotificationService(IWebhookNotificationRepository webhookNotificationRepository, IUriService uriService, 
            IOptionsSnapshot<AppSettings> appSettings, ISubscriptionRepository subscriptionRepository, INotificationService notificationService, 
            ITeamSubscriptionRepository teamSubscriptionRepository, ITeamRepository teamRepository, Bugsnag.IClient bugsnag)
        {
            _webhookNotificationRepository = webhookNotificationRepository;
            _appSettings = appSettings.Value;
            _uriService = uriService;
            _subscriptionRepository = subscriptionRepository;
            _notificationService = notificationService;
            _teamSubscriptionRepository = teamSubscriptionRepository;
            _teamRepository = teamRepository;
            _bugsnag = bugsnag;
        }


        public async Task<PagedResponseHandler<List<WebhookNotificationTraceLog>>> GetAllNotificationLogsAsync(PaginationFilter filter, string route)
        {
            var validFilters = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var logs = await _webhookNotificationRepository.GetAllPaginatedAsync(filter);

            var response = PaginationHelper.CreatePagedResponse<WebhookNotificationTraceLog>(logs.Records!, validFilters, logs.TotalCount, _uriService, route);

            response.Success = true;
            response.Message = "Webhook notification logs retrieved successfully";

            return response;
        }

        public async Task<ResponseHandler<string>> ProcessStripeWebhookNotificationAsync(HttpContext httpContext)
        {
            ResponseHandler<string> response = new();

            var json = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
            Event stripeEvent;

            try
            {
                var signatureHeader = httpContext.Request.Headers["Stripe-Signature"];
                //stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, _appSettings.StripeWebhookSecret);
                stripeEvent = EventUtility.ParseEvent(json);

                var log = await _webhookNotificationRepository.AddAsync(new WebhookNotificationTraceLog
                {
                    RequestData = json,
                    EventType = stripeEvent.Type,
                });

                bool result = false;
                switch (stripeEvent.Type)
                {
                    case Events.CheckoutSessionCompleted:
                        var checkoutSession = stripeEvent.Data.Object as Stripe.Checkout.Session;
                        result = await HandleCheckoutSessionCompletedNotificationAsync(stripeEvent.Id,stripeEvent.Type,checkoutSession!);                        
                        break;
                    case Events.InvoicePaid:
                        var invoicePaidObj = stripeEvent.Data.Object as Stripe.Invoice;
                        result = await HandleInvoicePaidNotificationAsync(stripeEvent.Id, stripeEvent.Type, invoicePaidObj!);
                        break;
                    case Events.InvoicePaymentFailed:
                        break;
                    case Events.CustomerSubscriptionTrialWillEnd:
                        var subscriptionObj = stripeEvent.Data.Object as Stripe.Subscription;
                        result = await HandleCustomerSubscriptionTrialEnd(stripeEvent.Id, stripeEvent.Type, subscriptionObj!);
                        break;
                    default:
                        response.Message = string.Format("Unhandled event type: {0}", stripeEvent.Type);
                        break;

                }              

                log.ResponseData = JsonConvert.SerializeObject(response);
                await _webhookNotificationRepository.UpdateAsync(log.Id, log);


                response.Success = result;
                response.Message = result ? "Webhook notification processed successfully" : "Could not process webhook notification";
                return response;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return response;
            }
        }

        public async Task<bool> HandleCheckoutSessionCompletedNotificationAsync(string eventId, string eventName, Stripe.Checkout.Session checkoutSession)
        {
            try
            {
                // get initial checkout session and create team subscription
                var subscriptionTransaction = await _subscriptionRepository.GetSubscriptionTransactionBySessionIdAsync(checkoutSession.Id)
                    ?? throw new ApplicationException("Checkout session not found");


                subscriptionTransaction.StripeSubscriptionId = checkoutSession.SubscriptionId;
                subscriptionTransaction.StripeCustomerId = checkoutSession.CustomerId;
                subscriptionTransaction.StripeInvoiceId = checkoutSession.InvoiceId;
                subscriptionTransaction.StripeEventId = eventId;
                subscriptionTransaction.StripeEventName = eventName;
                subscriptionTransaction.IsCheckoutTransaction = true;
                subscriptionTransaction.EventDataObject = JsonConvert.SerializeObject(checkoutSession, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.None,
                });

                var updateSubTransaction = 
                    await _subscriptionRepository.UpdateSubscriptionTransactionAsync(subscriptionTransaction.Id, subscriptionTransaction, SubscriptionTransactionUpdateAction.checkout_session_completed);
                // create team subscription

                var teamSubscription = new TeamSubscription
                {
                    TeamId = subscriptionTransaction.TeamId,
                    SubscriptionId = subscriptionTransaction.SubscriptionId,
                    SubscriptionTransactionId = subscriptionTransaction.Id.ToString(),
                    CreatedAt = DateTime.Now,
                    
                };


                var newTeamSubscription = await _teamSubscriptionRepository.AddAsync(teamSubscription);

                var teamAdmin = await _teamRepository.GetTeamAdmin((Guid)teamSubscription.TeamId!);

                // send notification to teamAdmin
                await _notificationService.SendCustomNotificationAsync(new DTOs.Requests.NotificationDto
                {
                    RecipientEmail = teamAdmin.Email,
                    RecipientName = $"{teamAdmin.Firstname} {teamAdmin.Lastname}",
                    Event = NotificationEvent.subscription_checkout_session_completed
                    

                }, _appSettings);


                return true;
            }catch(Exception ex)
            {
                _bugsnag.Notify(ex);
                return false;
            }
        }
        
        public async Task<bool> HandleInvoicePaidNotificationAsync(string eventId, string eventName, Stripe.Invoice invoice)
        {
            try
            {
                // get subscription transaction by invoice id
                var subscriptionTransaction = await _subscriptionRepository.GetSubscriptionTransactionByStripeRecIdAsync(invoice.Id, "invoice");

                if(subscriptionTransaction == null)
                {
                    // get new subscription transaction
                    var existingSubscriptionTransaction = await _subscriptionRepository.GetSubscriptionTransactionByStripeRecIdAsync(invoice.SubscriptionId, "subscription") 
                        ?? throw new ApplicationException("Could not identify payment");

                    subscriptionTransaction = await _teamRepository.AddTeamSubscriptionTransactionAsync(new Models.SubscriptionModels.SubscriptionTransactions
                    {
                        TeamId = (Guid)existingSubscriptionTransaction.TeamId!,
                        SubscriptionId = existingSubscriptionTransaction.SubscriptionId,
                        StripeSubscriptionId = invoice.SubscriptionId,
                        StripeCustomerId = invoice.CustomerId,
                        StripeEventId = eventId,
                        StripeEventName = eventName,
                        StripeInvoiceId = invoice.Id,
                        EventDataObject = JsonConvert.SerializeObject(invoice, new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            Formatting = Formatting.None,
                        })
                    });
                }
                else
                {
                    subscriptionTransaction.StripeInvoiceId = invoice.Id;
                    var updateSubTransaction =
                        await _subscriptionRepository.UpdateSubscriptionTransactionAsync(subscriptionTransaction.Id, subscriptionTransaction, SubscriptionTransactionUpdateAction.invoice_paid);
                }


                // create team subscription
                var teamSubscription = new TeamSubscription
                {
                    TeamId = subscriptionTransaction.TeamId,
                    SubscriptionId = subscriptionTransaction.SubscriptionId,
                    SubscriptionTransactionId = subscriptionTransaction.Id.ToString(),
                    StartDate = invoice.Lines.First().Period.Start, 
                    EndDate = invoice.Lines.First().Period.End,
                    StripeSubscriptionId = invoice.SubscriptionId
                };


                var updatedTeamSubscription = await _teamSubscriptionRepository.UpdateTeamSubscriptionAsync(teamSubscription);

                var teamAdmin = await _teamRepository.GetTeamAdmin((Guid)teamSubscription.TeamId!);

                // send notification to teamAdmin
                await _notificationService.SendCustomNotificationAsync(new DTOs.Requests.NotificationDto
                {
                    RecipientEmail = teamAdmin.Email,
                    RecipientName = $"{teamAdmin.Firstname} {teamAdmin.Lastname}",
                    Event = NotificationEvent.subscription_payment_completed,
                    Additional = $@"<p>Click this link to Download your invoice and/or receipt. <a href=""{ invoice.HostedInvoiceUrl }"">Click Here</a></p>"                    

                }, _appSettings);


                return true;
            }catch(Exception ex)
            {
                _bugsnag.Notify(ex);
                return false;
            }
        }

        public async Task<bool> HandleCustomerSubscriptionTrialEnd(string eventId, string eventName, Stripe.Subscription subscription)
        {
            try
            {
                // get subscription transaction by invoice id
                var subscriptionTransaction = await _subscriptionRepository.GetSubscriptionTransactionByStripeRecIdAsync(subscription.Id, "subscription")
                    ?? throw new ApplicationException("Could not identify subscription");


                var teamAdmin = await _teamRepository.GetTeamAdmin(subscriptionTransaction.TeamId);

                // send notification to teamAdmin
                await _notificationService.SendCustomNotificationAsync(new DTOs.Requests.NotificationDto
                {
                    RecipientEmail = teamAdmin.Email,
                    RecipientName = $"{teamAdmin.Firstname} {teamAdmin.Lastname}",
                    Event = NotificationEvent.subscription_trial_will_end,
                    Additional = $@"<p>Click this link to make payment and activate your subscription <a href=""{subscription.LatestInvoice.HostedInvoiceUrl}"">Click Here</a></p>"

                }, _appSettings);


                return true;
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                return false;
            }
        }
    }
}
