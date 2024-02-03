using _750HrsTracker.Models.ActivityLogModels;
using _750HrsTracker.Models.JointEntities;
using System.ComponentModel.DataAnnotations;

namespace _750HrsTracker.Models
{
    public class Team
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }    
        public Guid? OwnerId { get; set; }
        public User? Owner { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public ICollection<AvailableProperty>? Properties { get; set; }
        public ICollection<TeamUser>? TeamUsers { get; set; }
        public ICollection<PropertyTeamUser>? PropertyTeamUsers { get; set; }
        public ICollection<ActivityLog>? ActivityLogs { get; set; }
    }
}
