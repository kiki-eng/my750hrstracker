using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Models.ResponseWrappers;

namespace _750HrsTracker.Services.Interfaces
{
    public interface ITeamService
    {
        Task<ResponseHandler<string>> InviteUserAsync(InviteUserRequest request, HttpRequest httpRequest);
        Task<ResponseHandler<string>> ValidateInvitationAsync(ValidateInvitationRequest request);
        Task<ResponseHandler<string>> CreateInvitedUserAsync(CreateInvitedUserRequest request);
        Task<ResponseHandler<List<PendingUserInvitationResponse>>> GetPendingUserInvitationsAsync();
    }
}
