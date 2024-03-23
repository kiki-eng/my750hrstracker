using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Enums;
using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;

namespace _750HrsTracker.Services.Interfaces
{
    public interface IActivityLogActivityService
    {
        Task<ResponseHandler<GetActivityLogActivityResponse>> AddActivityLogActivityAsync(AddUpdateActivityLogActivityRequest request);
        Task<ResponseHandler<GetActivityLogActivityResponse>> GetActivityLogActivityAsync(Guid id);
        Task<PagedResponseHandler<List<GetActivityLogActivityResponse>>> GetAllActivityLogActivityAsync(PaginationFilter filter, string route);
        Task<ResponseHandler<List<GetActivityLogActivityResponse>>> GetAllActivityLogActivityAsync(AvailablePropertyType propertyType);
        Task<ResponseHandler<List<GetActivityLogActivityResponse>>> SearchActivityLogActivityAsync(string keyword);
        Task<ResponseHandler<GetActivityLogActivityResponse>> UpdateActivityLogActivityAsync(Guid id, AddUpdateActivityLogActivityRequest request);
        Task<ResponseHandler<string>> DeleteActivityLogActivityAsync(Guid id);
        
        
        
        Task<ResponseHandler<GetLogActivitySubCategoryResponse>> AddLogActivitySubCategoryAsync(Guid logActivityId, AddUpdateLogActivitySubCategoryRequest request);
        Task<ResponseHandler<GetLogActivitySubCategoryResponse>> GetLogActivitySubCategoryAsync(Guid id);
        Task<PagedResponseHandler<List<GetLogActivitySubCategoryResponse>>> GetAllLogActivitySubCategoryAsync(PaginationFilter filter, string route);
        Task<PagedResponseHandler<List<GetLogActivitySubCategoryResponse>>> GetAllLogActivitySubCategoryAsync(Guid logActivityId, PaginationFilter filter, string route);
        Task<ResponseHandler<List<GetLogActivitySubCategoryResponse>>> SearchLogActivitySubCategoryAsync(string keyword);
        Task<ResponseHandler<GetLogActivitySubCategoryResponse>> UpdateLogActivitySubCategoryAsync(Guid id, AddUpdateLogActivitySubCategoryRequest request);
        Task<ResponseHandler<string>> DeleteLogActivitySubCategoryAsync(Guid id);




    }
}

