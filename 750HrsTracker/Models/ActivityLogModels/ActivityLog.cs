using _750HrsTracker.Enums;
using _750HrsTracker.Models.JointEntities;

namespace _750HrsTracker.Models.ActivityLogModels
{
    public class ActivityLog : BaseEntity
    {
        public string? Name { get; set; }
        public DateTime ActivityDate { get; set; }
        public int HoursSpent { get; set; }
        public int MinutesSpent { get; set; }
        public int SecondsSpent { get; set; }
        public string? Description { get; set; }
        public Guid? TeamId { get; set; }
        public Team? Team { get; set; }
        public Guid ActivityById { get; set; }
        public User? ActivityBy { get; set; }
        
        public Guid CreatedById { get; set; }
        public User? CreatedBy { get; set; }

        public Guid? ActivityLogActivityId { get; set; }
        public ActivityLogActivity? ActivityLogActivity { get; set; }
        
        public Guid? ActivityLogCategoryId { get; set; }
        public ActivityLogCategory? ActivityLogCategory { get; set; }

        public Guid? TaskId { get; set; }
        public ActivityLogSubCategory? Task { get; set; }

        public AvailablePropertyType PropertyType { get; set; }
        public ActivityLogType LogType { get; set; }
        public ICollection<ActivityLogDocument>? ActivityLogDocuments { get; set; }
        public ICollection<ActivityLogProperty>? ActivityLogProperties { get; set; }
    }
}
