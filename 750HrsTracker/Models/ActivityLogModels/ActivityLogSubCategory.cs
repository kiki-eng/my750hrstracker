using _750HrsTracker.Enums;

namespace _750HrsTracker.Models.ActivityLogModels
{
    public class ActivityLogSubCategory : BaseEntity
    {
        public string? Name { get; set; }
        public string? Slug { get; set; }

        public Guid? LogActivityId { get; set; }
        public ActivityLogActivity? LogActivity { get; set; }

        public ICollection<ActivityLog>? ActivityLogs { get; set; }
    }
}
