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
        public WebhookNotificationService(IWebhookNotificationRepository webhookNotificationRepository, IUriService uriService, 
            IOptionsSnapshot<AppSettings> appSettings, ISubscriptionRepository subscriptionRepository, INotificationService notificationService, 
            ITeamSubscriptionRepository teamSubscriptionRepository, ITeamRepository teamRepository)
        {
            _webhookNotificationRepository = webhookNotificationRepository;
            _appSettings = appSettings.Value;
            _uriService = uriService;
            _subscriptionRepository = subscriptionRepository;
            _notificationService = notificationService;
            _teamSubscriptionRepository = teamSubscriptionRepository;
            _teamRepository = teamRepository;
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


                switch (stripeEvent.Type)
                {
                    case Events.CheckoutSessionCompleted:
                        var checkoutSession = stripeEvent.Data.Object as Stripe.Checkout.Session;
                        await HandleCheckoutSessionCompletedNotificationAsync(checkoutSession!);
                        break;
                    case Events.InvoicePaid:
                        break;
                    case Events.InvoicePaymentFailed:
                        break;
                    case Events.CustomerSubscriptionTrialWillEnd:
                        break;
                    default:
                        response.Message = string.Format("Unhandled event type: {0}", stripeEvent.Type);
                        break;

                }              

                log.ResponseData = JsonConvert.SerializeObject(response);
                await _webhookNotificationRepository.UpdateAsync(log.Id, log);

                return response;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return response;
            }
        }

        public async Task HandleCheckoutSessionCompletedNotificationAsync(Stripe.Checkout.Session checkoutSession)
        {
            // get initial checkout session and create team subscription
            var subscriptionTransaction = await _subscriptionRepository.GetSubscriptionTransactionBySessionIdAsync(checkoutSession.Id) 
                ??  throw new ApplicationException("Checkout session not found");

            // create team subscription
            var teamSubscription = new TeamSubscription
            {
                TeamId = subscriptionTransaction.TeamId,
                SubscriptionId = subscriptionTransaction.SubscriptionId,
                StillOnTrial = true,
                TrialStartDate = checkoutSession.Subscription.TrialStart,
                TrialEndDate = checkoutSession.Subscription.TrialEnd,
                CreatedAt = DateTime.Now,
                
            };

            var newTeamSubscription = await _teamSubscriptionRepository.AddAsync(teamSubscription);

            var teamAdmin = await _teamRepository.GetTeamAdmin((Guid)teamSubscription.TeamId!);

            // send notification to teamAdmin
            await _notificationService.SendCustomNotificationAsync(new DTOs.Requests.NotificationDto
            {
                RecipientEmail = teamAdmin.Email,
                RecipientName = $"{teamAdmin.Firstname} {teamAdmin.Lastname}"

            }, _appSettings);

        }
    }
}
