namespace _750HrsTracker.Models.SubscriptionModels
{
    public class TeamSubscription : BaseEntity
    {
        public Guid? TeamId { get; set; }
        public Team? Team { get; set; }
        public Guid? SubscriptionId { get; set; }
        public Subscription? Subscription { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int GracePeriodMinutes { get; set; }
        public bool StillOnTrial { get; set; }
        public DateTime? TrialStartDate { get; set; }
        public DateTime? TrialEndDate { get; set; }

    }
}
