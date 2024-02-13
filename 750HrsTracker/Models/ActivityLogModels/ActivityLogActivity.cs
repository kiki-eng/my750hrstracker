using _750HrsTracker.Enums;

namespace _750HrsTracker.Models.ActivityLogModels
{
    public class ActivityLogActivity : BaseEntity
    {
        public string? Name { get; set; }       
        public string? Slug { get; set; }

        public AvailablePropertyType AvailablePropertyType { get; set; }
        public Guid? ActivityLogCategoryId { get; set; }
        public ActivityLogCategory? ActivityLogCategory { get; set; }

        public ICollection<ActivityLog>? ActivityLogs { get; set; } 
        public ICollection<ActivityLogSubCategory>? ActivityLogSubCategories { get; set; } 
    }
}
