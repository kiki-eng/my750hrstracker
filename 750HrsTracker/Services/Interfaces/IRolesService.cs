using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;

namespace _750HrsTracker.Services.Interfaces
{
    public interface IRolesService
    {
        Task<PagedResponseHandler<List<GetRoleResponse>>> GetAllRolesAsync(PaginationFilter filter, string route);
        Task<ResponseHandler<GetRoleResponse>> UpdateRoleAsync(Guid id, AddUpdateRolesRequest request);
        Task<ResponseHandler<string>> DeleteRoleAsync(Guid id);
    }
}
