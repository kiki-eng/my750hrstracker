using _750HrsTracker.Models.SubscriptionModels;

namespace _750HrsTracker.Models
{
    public class Permission : BaseEntity
    {
        public string? Name { get; set; }
        public string? Value { get; set; }
        public string? Slug { get; set; }
        public string? Module { get; set; }

        public ICollection<SubscriptionPermission>? SubscriptionPermissions { get; set; }


    }
}
