using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;

namespace _750HrsTracker.Services.Interfaces
{
    public interface IActivityLogSubCategoryService
    {              
        
        Task<ResponseHandler<GetLogActivitySubCategoryResponse>> AddLogActivitySubCategoryAsync(Guid logActivityId, AddUpdateLogActivitySubCategoryRequest request);
        Task<ResponseHandler<GetLogActivitySubCategoryResponse>> GetLogActivitySubCategoryAsync(Guid id);
        Task<PagedResponseHandler<List<GetLogActivitySubCategoryResponse>>> GetAllLogActivitySubCategoryAsync(PaginationFilter filter, string route);
        Task<PagedResponseHandler<List<GetLogActivitySubCategoryResponse>>> GetAllLogActivitySubCategoryAsync(Guid logActivityId, PaginationFilter filter, string route);
        Task<ResponseHandler<List<GetLogActivitySubCategoryResponse>>> SearchLogActivitySubCategoryAsync(string keyword);
        Task<ResponseHandler<GetLogActivitySubCategoryResponse>> UpdateLogActivitySubCategoryAsync(Guid id, AddUpdateLogActivitySubCategoryRequest request);
        Task<ResponseHandler<string>> DeleteLogActivitySubCategoryAsync(Guid id);

    }
}

