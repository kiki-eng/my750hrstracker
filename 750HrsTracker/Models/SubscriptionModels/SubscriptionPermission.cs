namespace _750HrsTracker.Models.SubscriptionModels
{
    public class SubscriptionPermission : BaseEntity
    {
        public Guid SubscriptionId { get; set; }
        public Subscription? Subscription { get; set; }
        public Guid PermissionId { get; set; }
        public Permission? Permission { get; set; }
    }
}
