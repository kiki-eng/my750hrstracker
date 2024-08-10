using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Enums;
using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;

namespace _750HrsTracker.Services.Interfaces
{
    public interface IAdminService
    {
        Task<PagedResponseHandler<List<GetTeamResponse>>> GetAllTeamsAsync(PaginationFilter filter, HttpRequest httpRequest);
        Task<ResponseHandler<string>> SendSupportNotificationAsync(SupportRequest supportRequest);
        Task<ResponseHandler<List<GetSubscriptionResponse>>> GetSubscriptionsAsync();
        Task<ResponseHandler<GetAutoSuggestionResponse>> GetAutoSuggestionResponseAsync(string keyword);

        // activity logs
        Task<ResponseHandler<AdminGetActivityLogResponse>> GetActivityLogAsync(Guid id);
        Task<PagedResponseHandler<List<AdminGetActivityLogResponse>>> GetAllActivityLogAsync(AvailablePropertyType propertyType, PaginationFilter filter, ActivityLogFilter activityLogFilter, string route);
        Task<ResponseHandler<List<AdminGetActivityLogResponse>>> SearchActivityLogAsync(string keyword);

        // properties
        Task<ResponseHandler<AdminGetPropertyResponse>> GetPropertyAsync(Guid propertyId);
        Task<PagedResponseHandler<List<AdminGetPropertyResponse>>> GetAllPropertiesAsync(PaginationFilter filter, string route);
        Task<ResponseHandler<List<AdminGetPropertyResponse>>> SearchPropertiesAsync(string keyword); 
    }
}
