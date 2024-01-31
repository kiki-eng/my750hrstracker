using _750HrsTracker.Models.JointEntities;
using Microsoft.AspNetCore.Identity;

namespace _750HrsTracker.Models
{
    public class User : IdentityUser<Guid>
    {
        public string? Firstname { get; set; }   
        public string? Lastname { get; set; }

        public bool IsActive { get; set; }
        public bool FirstTime { get; set; }
        public bool SendLoginNotification { get; set; }

        public string? DefaultTeamId { get; set; }

        public DateTime CreatedAt{ get; set; }
        public DateTime ModifiedAt{ get; set; }

        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public string? VerificationToken { get; set; }
        public DateTime? VerificationTokenExpires { get; set; }
        public DateTime? LastPasswordResetAt { get; set; }


        public ICollection<AvailableProperty>? PropertiesCreated { get; set; }
        public ICollection<TeamUser>? UserTeams { get; set; }
        public ICollection<PropertyTeamUser>? PropertyTeamUsers { get; set; }


        public User()
        {
            CreatedAt = DateTime.Now;
            ModifiedAt = DateTime.Now;
            IsActive = true;
            FirstTime = true;
            SendLoginNotification = true;
        }
    }
}
