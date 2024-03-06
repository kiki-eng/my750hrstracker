using _750HrsTracker.Enums;

namespace _750HrsTracker.Models.ActivityLogModels
{
    public class ActivityLogCategory : BaseEntity
    {
        public string? Name { get; set; }
        public string? Slug { get; set; }

        public AvailablePropertyType? AvailablePropertyType { get; set; }
        public ICollection<ActivityLog>? ActivityLogs { get; set; }
        public ICollection<ActivityLogActivity>? ActivityLogActivities { get; set;}
    }
}
