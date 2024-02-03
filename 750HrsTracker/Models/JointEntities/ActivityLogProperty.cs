using _750HrsTracker.Models.ActivityLogModels;

namespace _750HrsTracker.Models.JointEntities
{
    public class ActivityLogProperty : BaseEntity
    {
        public Guid PropertyId { get; set; }
        public AvailableProperty? Property { get; set; }
        public Guid ActivityLogId { get; set; }
        public ActivityLog? ActivityLog { get; set; }
    }
}
