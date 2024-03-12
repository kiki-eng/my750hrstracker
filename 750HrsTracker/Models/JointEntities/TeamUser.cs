namespace _750HrsTracker.Models.JointEntities
{
    public class TeamUser
    {
        public Guid TeamId { get; set; }
        public Team? Team { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public bool IsOwnerSpouse { get; set; }
        public bool IsActive { get; set; }

    }
}
