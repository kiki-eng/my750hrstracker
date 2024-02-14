using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Filters;
using _750HrsTracker.Helpers;
using _750HrsTracker.Models;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Models.SubscriptionModels;
using _750HrsTracker.Repositories.Implementations;
using _750HrsTracker.Repositories.Interfaces;
using _750HrsTracker.Services.Interfaces;
using AutoMapper;

namespace _750HrsTracker.Services.Implementations
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        public SubscriptionService(ISubscriptionRepository subscriptionRepository, IPermissionRepository permissionRepository,
            IMapper mapper, IUriService uriService)
        {
            _subscriptionRepository = subscriptionRepository;
            _permissionRepository = permissionRepository;
            _mapper = mapper;
            _uriService = uriService;
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

        public async Task<ResponseHandler<List<GetSubscriptionResponse>>> GetAllSubscriptionAsync()
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
    }

}
