using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;

namespace _750HrsTracker.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Task<ResponseHandler<GetSubscriptionResponse>> AddSubscriptionAsync(AddUpdateSubscriptionRequest request);
        Task<ResponseHandler<GetSubscriptionResponse>> GetSubscriptionAsync(Guid subscriptionId);
        Task<PagedResponseHandler<List<GetSubscriptionResponse>>> GetAllSubscriptionAsync(PaginationFilter filter, string route);
        Task<ResponseHandler<List<GetSubscriptionResponse>>> GetAllSubscriptionAsync();
        Task<ResponseHandler<GetSubscriptionResponse>> UpdateSubscriptionAsync(Guid id, AddUpdateSubscriptionRequest request);
        Task<ResponseHandler<string>> DeleteSubscriptionAsync(Guid id);
    }
}
