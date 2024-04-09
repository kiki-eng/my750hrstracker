using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;

namespace _750HrsTracker.Services.Interfaces
{
    public interface IAdminService
    {
        Task<PagedResponseHandler<List<GetTeamResponse>>> GetAllTeamsAsync(PaginationFilter filter, HttpRequest httpRequest);
        Task<ResponseHandler<string>> SendSupportNotificationAsync(SupportRequest supportRequest);
        Task<ResponseHandler<List<GetSubscriptionResponse>>> GetSubscriptionsAsync();
    }
}
