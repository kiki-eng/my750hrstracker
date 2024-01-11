using _750HrsTracker.Models.JointEntities;
using Microsoft.AspNetCore.Identity;

namespace _750HrsTracker.Models
{
    public class User : IdentityUser<Guid>
    {
        public string? Firstname { get; set; }   
        public string? Lastname { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt{ get; set; }
        public DateTime ModifiedAt{ get; set; }

        public ICollection<TeamUser>? UserTeams { get; set; }

        public User()
        {
            CreatedAt = DateTime.Now;
            ModifiedAt = DateTime.Now;
            IsActive = true;
        }
    }
}
