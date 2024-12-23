using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Enums;
using _750HrsTracker.Filters;
using _750HrsTracker.Models.ActivityLogModels;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace _750HrsTracker.Services.Interfaces
{
    public interface IActivityLogService
    {
        Task<ResponseHandler<GetDashboardResponse>> GetDashboardDataAsync(AvailablePropertyType availablePropertyType, DasboardSummaryType summaryType = DasboardSummaryType.TEAM);
        Task<ResponseHandler<GetActivityLogResponse>> AddActivityLogAsync(AddActivityLogRequest request);
        Task<ResponseHandler<GetActivityLogResponse>> AddSTRActivityLogAsync(BaseAddActivityLogRequest request);
        Task<ResponseHandler<GetActivityLogResponse>> GetActivityLogAsync(Guid id);
        Task<LogReportDownloadResponse> DownloadActivityLogReportAsync(AvailablePropertyType propertyType, PaginationFilter filter, ActivityLogFilter activityLogFilter);
        Task<PagedResponseHandler<List<GetActivityLogResponse>>> GetAllActivityLogAsync(AvailablePropertyType propertyType, PaginationFilter filter, ActivityLogFilter activityLogFilter, string route);
        Task<ResponseHandler<List<GetActivityLogResponse>>> SearchActivityLogAsync(string keyword, AvailablePropertyType propertyType);
        Task<ResponseHandler<GetActivityLogResponse>> UpdateActivityLogAsync(Guid id, AddActivityLogRequest request);
        Task<ResponseHandler<string>> DeleteActivityLogAsync(Guid id);
        Task<ResponseHandler<Base64FileModel>> DownloadActivityLogImportTemplateAsync();
        Task<ResponseHandler<string>> ImportActivityLogAsync(AvailablePropertyType propertyType, ImportActivityLogRequest request);

        Task<byte[]> ExportDocumentsAsync(ExportDocumentFilter filter);
    }
}
