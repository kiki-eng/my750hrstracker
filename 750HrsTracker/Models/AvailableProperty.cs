using _750HrsTracker.Models.JointEntities;
using System.ComponentModel.DataAnnotations;

namespace _750HrsTracker.Models
{
    public class AvailableProperty : BaseEntity
    {

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Alias { get; set; }

        public string? Longitude { get; set; }
        public string? Latitude { get; set; }   

        public Guid CreatedById {  get; set; } 
        public User? CreatedBy { get; set; }

        public Guid TeamId { get; set; }
        public Team? Team { get; set; }

        public ICollection<PropertyTeamUser>? PropertyTeamUsers { get; set; }

    }
}
