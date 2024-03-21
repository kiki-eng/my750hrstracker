using System.ComponentModel.DataAnnotations;

namespace _750HrsTracker.Models.JointEntities
{
    public class PropertyTeamUser : BaseEntity
    {
        public Guid? PropertyId { get; set; }
        public AvailableProperty? Property { get; set; }

        public Guid? TeamId { get; set; }
        public Team? Team { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
