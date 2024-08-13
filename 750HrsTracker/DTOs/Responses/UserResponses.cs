using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.Models.JointEntities;
using _750HrsTracker.Models.SubscriptionModels;

namespace _750HrsTracker.DTOs.Responses
{
    public class SignInResponse
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string? Token { get; set; }
        public DateTime TokenExpireAt { get; set; }
    }

    public class GetUsersOnlyResponse
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? DefaulTeamId { get; set; }

        public bool SendLoginNotification { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<GetRolesOnlyResponse>? Roles { get; set; }
        public Base64FileModel? ProfilePic { get; set; }

        public SubscriptionData? Subscription { get; set; }
    }

    public class GetUserPermissionsResponse
    {
        public Guid UserId { get; set; }
        public Guid TeamId { get; set; }
        public List<GetPermissionResponse>? Permissions { get; set; }
    }
    public class SubscriptionData
    {
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public TeamSubscription? TeamSubscription { get; set; }
    }
    public class InviteUserResponse
    {
        public bool InvitationSent { get; set; }
    }

    public class PendingUserInvitationResponse
    {
        public string? InvitedEmail { get; set; }
        public string? InviterEmail { get; set; }
        public string? InviterName { get; set; }
        public string? RoleName { get; set; }
        public string? RoleId { get; set; }
        public DateTime InvitedAt { get; set; }

    }
}
