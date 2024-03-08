using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Enums;
using _750HrsTracker.Filters;
using _750HrsTracker.Models.ActivityLogModels;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Repositories.Interfaces;

namespace _750HrsTracker.Services.Interfaces
{
    public interface IActivityLogService
    {
        Task<ResponseHandler<GetDashboardResponse>> GetDashboardDataAsync(AvailablePropertyType availablePropertyType);
        Task<ResponseHandler<GetActivityLogResponse>> AddActivityLogAsync(AddActivityLogRequest request);
        Task<ResponseHandler<GetActivityLogResponse>> GetActivityLogAsync(Guid id);
        Task<PagedResponseHandler<List<GetActivityLogResponse>>> GetAllActivityLogAsync(PaginationFilter filter, string route);
        Task<ResponseHandler<List<GetActivityLogResponse>>> SearchActivityLogAsync(string keyword);
        Task<ResponseHandler<GetActivityLogResponse>> UpdateActivityLogAsync(Guid id, UpdateActivityLogRequest request);
        Task<ResponseHandler<string>> DeleteActivityLogAsync(Guid id);
        Task<ResponseHandler<Base64FileModel>> DownloadActivityLogImportTemplateAsync();


    }
}
