using _750HrsTracker.Models.JointEntities;

namespace _750HrsTracker.Models
{
    public class Team
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }    
        public Guid? OwnerId { get; set; }
        public User? Owner { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public ICollection<TeamUser>? TeamUsers { get; set; }
    }
}
