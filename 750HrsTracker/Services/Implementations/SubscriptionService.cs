using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Enums;
using _750HrsTracker.Filters;
using _750HrsTracker.Helpers;
using _750HrsTracker.Helpers.Constants;
using _750HrsTracker.Models;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Models.SubscriptionModels;
using _750HrsTracker.Repositories.Implementations;
using _750HrsTracker.Repositories.Interfaces;
using _750HrsTracker.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Stripe;
using Subscription = _750HrsTracker.Models.SubscriptionModels.Subscription;

namespace _750HrsTracker.Services.Implementations
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly AppSettings _appSettings;
        private readonly Bugsnag.IClient _bugsnag;
        private readonly IUserRepository _userRepository;
        private readonly ITeamSubscriptionRepository _teamSubscriptionRepository;
        private readonly ITeamRepository _teamRepository;
        public SubscriptionService(ISubscriptionRepository subscriptionRepository, IPermissionRepository permissionRepository,
            IMapper mapper, IUriService uriService, IOptionsSnapshot<AppSettings> appSettings, Bugsnag.IClient bugsnag, 
            IUserRepository userRepository, ITeamSubscriptionRepository teamSubscriptionRepository, ITeamRepository teamRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _permissionRepository = permissionRepository;
            _mapper = mapper;
            _uriService = uriService;
            _appSettings = appSettings.Value;
            _bugsnag = bugsnag;
            _userRepository = userRepository;
            _teamSubscriptionRepository = teamSubscriptionRepository;
            _teamRepository = teamRepository;
        }

        public async Task<ResponseHandler<GetSubscriptionResponse>> AddSubscriptionAsync(AddUpdateSubscriptionRequest request)
        {
            ResponseHandler<GetSubscriptionResponse> response = new();

            var requestData = _mapper.Map<Subscription>(request);
            requestData.Slug = Utility.GenerateSlug(request.Name!);
            var subscription = await _subscriptionRepository.AddAsync(requestData);

            response.Success = true;
            response.Message = "Subscription added successfully";
            response.Data = _mapper.Map<GetSubscriptionResponse>(subscription);

            return response;
        }
         
        public async Task<ResponseHandler<string>> DeleteSubscriptionAsync(Guid id)
        {
            ResponseHandler<string> response = new();

            var subscription = await _subscriptionRepository.DeleteAsync(s => s.Id == id);

            response.Success = true;
            response.Message = "Subscription deleted successfully";

            return response;
        }

        public async Task<PagedResponseHandler<List<GetSubscriptionResponse>>> GetAllSubscriptionAsync(PaginationFilter filter, string route)
        {
            var validFilters = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var properties = await _subscriptionRepository.GetAllPaginatedAsync(filter);


            var pagedData = (properties.Records!.Select(sn => _mapper.Map<GetSubscriptionResponse>(sn))).ToList();

            PagedResponseHandler<List<GetSubscriptionResponse>> response =
                PaginationHelper.CreatePagedResponse(pagedData, validFilters, properties.TotalCount, _uriService, route);

            response.Success = true;
            response.Message = "All subscriptions retrieved successfully";
            return response;
        }

        public async Task<ResponseHandler<GetSubscriptionResponse>> GetSubscriptionAsync(Guid subscriptionId)
        {
            ResponseHandler<GetSubscriptionResponse> response = new();

            var subscription = await _subscriptionRepository.GetSingleOrDefaultAsync(s => s.Id == subscriptionId);  

            response.Success = true;
            response.Message = "Subscription retrieved successfully";
            response.Data = _mapper.Map<GetSubscriptionResponse>(subscription);

            return response;    
        }


        public async Task<ResponseHandler<GetSubscriptionResponse>> UpdateSubscriptionAsync(Guid id, AddUpdateSubscriptionRequest request)
        {
            ResponseHandler<GetSubscriptionResponse> response = new();

            var requestData = _mapper.Map<Subscription>(request);
            requestData.Slug = Utility.GenerateSlug(request.Name!);

            var updatedSubscription = await _subscriptionRepository.UpdateAsync(id, requestData);

            response.Success = true;
            response.Message = "Subscriptions updated successfully";
            response.Data = _mapper.Map<GetSubscriptionResponse>(updatedSubscription);

            return response;
        }

        public async Task<ResponseHandler<List<GetSubscriptionResponse>>> GetAllSubscriptionAsync(HttpRequest httpRequest)
        {
            ResponseHandler<List<GetSubscriptionResponse>> response = new();

            var userData = Utility.GetUserIdFromToken(httpRequest);

            var user = await _userRepository.GetUserAsync(Guid.Parse(userData.Item1));

            var team = await _teamRepository.GetTeamAsync(Guid.Parse(user.DefaultTeamId!));


            var subscriptions = await _subscriptionRepository.GetAllAsync();

            if (team.UsedTrial)
            {
                subscriptions = subscriptions.Where(s => s.Slug!.ToLower() != "free-plan");
            }

            response.Success = true;
            response.Message = "Subscriptions retrieved successfully";
            response.Data = subscriptions.Select(s => _mapper.Map<GetSubscriptionResponse>(s)).ToList();

            return response;
        }
        
        public async Task<ResponseHandler<List<GetSubscriptionResponse>>> GetAllSubscriptionPubAsync()
        {
            ResponseHandler<List<GetSubscriptionResponse>> response = new();

            var subscriptions = await _subscriptionRepository.GetAllAsync();

            response.Success = true;
            response.Message = "Subscriptions retrieved successfully";
            response.Data = subscriptions.Select(s => _mapper.Map<GetSubscriptionResponse>(s)).ToList();

            return response;
        }

        public async Task<ResponseHandler<UpdateSubscriptionPermissionResponse>> UpdateSubscriptionPermissionsAsync(Guid subscriptionId, UpdateSubscriptionPermissionRequest request)
        {
            ResponseHandler<UpdateSubscriptionPermissionResponse> response = new();

            var subscriptionPermission = await _subscriptionRepository.UpdateSubscriptionPermissionsAsync(subscriptionId, request.Permissions!.Select(p => _mapper.Map<Permission>(p)).ToList());

            response.Success = true;
            response.Message = "Subscription permisisons updated successfully";

            return response;
        }

        public async Task<ResponseHandler<string>> UpdateSubscriptionFeaturesAsync(Guid id, List<string> features)
        {
            ResponseHandler<string> response = new();

            _ = await _subscriptionRepository.UpdateFeaturesAsync(id, features);

            response.Success = true; 
            response.Message = "Subscription features updated successfully";

            return response;
        }

        public async Task<ResponseHandler<GetSubscriptionResponse>> UpdateSubscriptionPriceAsync(Guid id, UpdateSubscriptionPriceRequest request)
        {
            ResponseHandler<GetSubscriptionResponse> response = new();

            var requestData = new Subscription { StripePriceId = request.StripePriceId};

            var updatedSubscription = await _subscriptionRepository.UpdatePriceIdAsync(id, requestData);

            response.Success = true;
            response.Message = "Subscriptions updated successfully";
            response.Data = _mapper.Map<GetSubscriptionResponse>(updatedSubscription);

            return response;
        }

        public async Task<ResponseHandler<string>> CancelSubscriptionAsync(HttpRequest httpRequest)
        {

            ResponseHandler<string> response = new();

            try
            {
                var userData = Utility.GetUserIdFromToken(httpRequest);


                var user = await _userRepository.GetUserAsync(Guid.Parse(userData.Item1));


                var teamSubscription = await _teamSubscriptionRepository.GetWthSubscription(Guid.Parse(user.DefaultTeamId!)) 
                    ?? throw new ApplicationException("No team subscription found");

                var service = new Stripe.SubscriptionService();

                var result = service.Cancel(teamSubscription.StripeSubscriptionId);

                if (result.Status != "canceled")
                {
                    response.Message = "Could not cancel subscription now. Please try again soon.";

                    return response;
                }


                // update subscription as canceled and inactive
                var updated = _teamSubscriptionRepository.UpdateTeamSubscriptionAsync(teamSubscription, Enums.TeamSubscriptionUpdateAction.cancel);


                response.Success = true;
                response.Message = "Team subscription canceled successfully";

                return response;

            }
            catch(Exception ex)
            {
                _bugsnag.Notify(ex);
                throw;                
            } 
          

        }

        public async Task<ResponseHandler<string>> ValidateInAppPurchaseReceiptAsync(ValidateInAppPurchaseReceiptRequest request, HttpRequest httpRequest)
        {
            ResponseHandler<string> response = new();

            try
            {
                var userData = Utility.GetUserIdFromToken(httpRequest);


                var user = await _userRepository.GetUserAsync(Guid.Parse(userData.Item1));


                var receiptData = new Dictionary<string, string>();

                var iosReceiptValidationUrl = Utility.ValidateAndFixUrl(_appSettings.IosReceiptValidationUrl!);
                var androidReceiptValidationUrl = Utility.ValidateAndFixUrl(_appSettings.AndroidReceiptValidationUrl!);

                if (request.DeviceType.Equals(DeviceType.ios))
                {
                    receiptData.Add("receipt-data", request.TransactionReceipt!);
                }
                else
                {
                    // prep request data for android
                }

                var uri = request.DeviceType.Equals(DeviceType.ios) ? new Uri(iosReceiptValidationUrl) : new Uri(androidReceiptValidationUrl);

                var baseUrl = uri.Scheme + "://" + uri.Host + ":" +uri.Port;
                var requestUri = uri.PathAndQuery;


                var subscription = await _subscriptionRepository.GetSubscriptionByPriceIdAsync(request.ProductId!,
                           request.DeviceType.Equals(DeviceType.ios) ? SubscriptionType.ios : SubscriptionType.android);

                if(subscription.Slug!.Equals(SubscriptionConstants.FreeplanSlug))
                {
                    var purchaseDate = DateTime.Now;
                    var subscriptionTransaction = await _teamRepository.AddTeamSubscriptionTransactionAsync(new SubscriptionTransactions
                    {
                        IsMobileInAppPurchase = true,
                        EventDataObject = "N/A",
                        LastActionById = user.Id,
                        PurchaseDate = DateTime.Now,
                        TransactionId = "N/A",
                        TeamId = Guid.Parse(user.DefaultTeamId!),
                        SubscriptionId = subscription.Id,
                    });

                    var teamSubscription = await _teamSubscriptionRepository.AddAsync(new TeamSubscription
                    {
                        IsActive = true,
                        StartDate = purchaseDate,
                        EndDate = purchaseDate.AddDays(subscription.DurationDays),
                        SubscriptionId = subscription.Id,
                        SubscriptionTransactionId = subscriptionTransaction.Id.ToString(),
                        TeamId = Guid.Parse(user.DefaultTeamId!)
                    });


                    await _teamRepository.UpdateTeamTrialSubscriptionAsync(Guid.Parse(user.DefaultTeamId!), true);
                }
                else
                {
                    DateTime purchaseDateTime = DateTime.Now;
                    string responseAsString = "N/A";
                    string transactionId = "N/A";



                    if (_appSettings.ValidateInAppPurchase)
                    {
                        var httpResponse = await Utility.MakeHttpRequest(receiptData, baseUrl, requestUri, HttpMethod.Post)
                        ?? throw new ApplicationException("Unable to complete validation request");

                        responseAsString = await httpResponse?.Content.ReadAsStringAsync()!;
                        if (httpResponse != null && httpResponse.IsSuccessStatusCode)
                        {
                            if (request.DeviceType.Equals(DeviceType.ios))
                            {
                                var responseData = JsonConvert.DeserializeObject<IosReceiptVerificationResponse>(responseAsString);

                                var subscriptionDetails = responseData?.Receipt?.InApp?.FirstOrDefault(ia => ia.TransactionId == request.TransactionId)
                                    ?? throw new ApplicationException($"Could not verify receipt - Status => {responseData?.Status}");


                                purchaseDateTime = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(subscriptionDetails.PurchaseDateMs)).DateTime;
                                transactionId = subscriptionDetails.TransactionId!;
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            response.Message = "Unable to validate subscription";

                            return response;
                        }
                    }


                    var subscriptionTransaction = await _teamRepository.AddTeamSubscriptionTransactionAsync(new SubscriptionTransactions
                    {
                        IsMobileInAppPurchase = true,
                        EventDataObject = responseAsString,
                        LastActionById = user.Id,
                        PurchaseDate = purchaseDateTime,
                        TransactionId = transactionId,
                        TeamId = Guid.Parse(user.DefaultTeamId!),
                        SubscriptionId = subscription.Id,
                    });

                    var teamSubscription = await _teamSubscriptionRepository.AddAsync(new TeamSubscription
                    {
                        IsActive = true,
                        StartDate = purchaseDateTime,
                        EndDate = purchaseDateTime.AddDays(subscription.DurationDays),
                        SubscriptionId = subscription.Id,
                        SubscriptionTransactionId = subscriptionTransaction.Id.ToString(),
                        TeamId = Guid.Parse(user.DefaultTeamId!)
                    });



                }

                response.Success = true;
                response.Message = "Receipt validated successfully";

                return response;

            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                throw;
            }

        }
    }

}
