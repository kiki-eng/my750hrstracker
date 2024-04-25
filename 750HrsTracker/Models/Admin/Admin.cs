using _750HrsTracker.Models.ActivityLogModels;
using _750HrsTracker.Models.JointEntities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace _750HrsTracker.Models.Admin
{
    public class Admin : IdentityUser<Guid>
    {
        public string? Firstname { get; set; }   
        public string? Lastname { get; set; }
        public bool IsActive { get; set; }
        public bool FirstTime { get; set; }
        public bool SendLoginNotification { get; set; }
        public DateTime CreatedAt{ get; set; }
        public DateTime ModifiedAt{ get; set; }

        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public string? VerificationToken { get; set; }
        public DateTime? VerificationTokenExpires { get; set; }
        public DateTime? LastPasswordResetAt { get; set; }

        public Guid? ProfilePictureId { get; set; }
        public AdminProfilePicture? ProfilePicture { get; set; }

        [NotMapped]
        public List<Role>? Roles { get; set; }
        public Admin()
        {
            CreatedAt = DateTime.Now;
            ModifiedAt = DateTime.Now;
            IsActive = true;
            FirstTime = true;
            SendLoginNotification = true;
        }
    }
}
