using System.ComponentModel.DataAnnotations.Schema;

namespace _750HrsTracker.Models.SubscriptionModels
{
    public class Subscription : BaseEntity
    {
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public decimal Price { get; set; }
        public int GracePeriodMinutes { get; set; }
        public string? Features { get; set; }

        public ICollection<SubscriptionPermission>? SubcriptionPermissions { get; set; }
        public ICollection<TeamSubscription>? TeamSubscriptions { get; set; }   
    }
}
