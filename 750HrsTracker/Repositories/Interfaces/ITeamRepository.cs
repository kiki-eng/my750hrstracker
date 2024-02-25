using _750HrsTracker.Models;

namespace _750HrsTracker.Repositories.Interfaces
{
    public interface ITeamRepository
    {
        Task<Role> AddTeamRoleAsync(Guid teamId, string roleName, Guid currentUserId, List<Permission> permissions);
        Task<List<Role>> GetTeamRolesAsync(Guid teamId);
        Task<Role> UpdateTeamRoleAsync(Guid teamId, Guid roleId, Guid currentUserId, Role role);
        Task<Role> DeleteRoleAsync(Guid teamId, Guid roleId);
        Task<Role> UpdateRolePermissionsAsync(Guid teamId, Guid roleId, Guid currentUserId, List<Permission> permissions);
        Task<UserInvitation> InviteUserAsync(Guid teamId, Guid userId, string inviteeEmail, Guid roleId);
        Task<UserInvitation> ValidateInvitationAsync(string inviteeEmail, string invitationCode);
        Task<bool> IsInvitedUserConfirmed(string inviteeEmail);
        Task<User> CreateInvitedUserAsync(User user, string invitationCode);
        Task<List<UserInvitation>> GetPendingUserInvitationsAsync(Guid teamId);
    }
}
