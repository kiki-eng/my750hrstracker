using _750HrsTracker.Models;

namespace _750HrsTracker.Repositories.Interfaces
{
    public interface ITeamRepository
    {
        Task<UserInvitation> InviteUserAsync(Guid teamId, Guid userId, string inviteeEmail, Guid roleId);
        Task<UserInvitation> ValidateInvitationAsync(string inviteeEmail, string invitationCode);
        Task<bool> IsInvitedUserConfirmed(string inviteeEmail);
        Task<User> CreateInvitedUserAsync(User user, string invitationCode);
        Task<List<UserInvitation>> GetPendingUserInvitationsAsync(Guid teamId);
    }
}
