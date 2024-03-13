using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;

namespace _750HrsTracker.Services.Interfaces
{
    public interface IActivityLogCategoryService
    {
        Task<ResponseHandler<GetActivityLogCategoryResponse>> AddActivityLogCategoryAsync(AddActivityLogCategoryRequest request);
        Task<ResponseHandler<GetActivityLogCategoryResponse>> GetActivityLogCategoryAsync(Guid id);
        Task<PagedResponseHandler<List<GetActivityLogCategoryResponse>>> GetAllActivityLogCategoryAsync(PaginationFilter filter, string route);
        Task<ResponseHandler<List<GetTimeAndLogCategoryResponse>>> GetAllActivityLogCategoryAsync();
        Task<ResponseHandler<List<GetActivityLogCategoryResponse>>> SearchActivityLogCategoryAsync(string keyword);
        Task<ResponseHandler<GetActivityLogCategoryResponse>> UpdateActivityLogCategoryAsync(Guid id, UpdateActivityLogCategoryRequest request);
        Task<ResponseHandler<string>> DeleteActivityLogCategoryAsync(Guid id);
    }
}

