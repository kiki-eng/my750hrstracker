using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;

namespace _750HrsTracker.Services.Interfaces
{
    public interface ITeamService
    {
        Task<ResponseHandler<GetRoleResponse>> AddTeamRoleAsync(AddRoleRequest request);
        Task<PagedResponseHandler<List<GetRoleResponse>>> GetTeamRolesAsync(PaginationFilter filter, string route);
        Task<ResponseHandler<GetRoleResponse>> UpdatetRoleAsync(Guid roleId, UpdateRoleRequest request);
        Task<ResponseHandler<string>> DeleteRoleAsync(Guid roleId);
        Task<ResponseHandler<string>> UpdateRolePermissionsAsync(Guid roleId, UpdateRolePermissionsRequest request);
        Task<ResponseHandler<string>> InviteUserAsync(InviteUserRequest request, HttpRequest httpRequest);
        Task<ResponseHandler<string>> ValidateInvitationAsync(ValidateInvitationRequest request);
        Task<ResponseHandler<string>> CreateInvitedUserAsync(CreateInvitedUserRequest request);
        Task<ResponseHandler<List<PendingUserInvitationResponse>>> GetPendingUserInvitationsAsync();
        Task<PagedResponseHandler<List<GetUserResponse>>> GetTeamUsersAsync(PaginationFilter filter, string route);
        Task<ResponseHandler<List<GetUserResponse>>> GetTeamUsersAsync();
        Task<ResponseHandler<GetUserResponse>> MaKeSpouseRequestAsync(MakeSpouseRequest request);
        Task<ResponseHandler<GetUserResponse>> ActivateDeactivateUsersAsync(MakeSpouseRequest request);
        Task<ResponseHandler<string>> DeactivateAccountAsync();

    }
}
