using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;

namespace _750HrsTracker.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<ResponseHandler<GetPermissionResponse>> AddPermissionAsync(AddUpdatePermissionRequest request);
        Task<ResponseHandler<GetPermissionResponse>> GetPermissionAsync(Guid permissionId);
        Task<PagedResponseHandler<List<GetPermissionResponse>>> GetAllPermissionAsync(PaginationFilter filter, string route);
        Task<ResponseHandler<GetPermissionResponse>> UpdatePermissionAsync(Guid id, AddUpdatePermissionRequest request);
        Task<ResponseHandler<string>> DeletePermissionAsync(Guid id);
    }
}
