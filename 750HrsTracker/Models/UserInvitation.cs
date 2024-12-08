using System.ComponentModel.DataAnnotations;

namespace _750HrsTracker.Models
{
    public class UserInvitation : BaseEntity
    {
        public string? Email { get; set; }
        public string? RoleName { get; set; }
        public string? RoleId { get; set; }
        public string? Code { get; set; }
        public Guid InviterId { get; set; }
        public string? InviterName { get; set; }
        public string? InviterEmail { get; set; }
        public Guid TeamId { get; set; }
        public string? TeamName { get; set; }
        public bool InvitationAccepted { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool ExistingUser { get; set; }

        public UserInvitation()
        {
            InvitationAccepted = false;
        }
    }
}
