using System.ComponentModel.DataAnnotations;

namespace _750HrsTracker.Models.SubscriptionModels
{
    public class SubscriptionTransactions
    {
        [Key]
        public Guid Id { get; set; }        

        public Guid TeamId { get; set; }
        public Team? Team { get; set; }

        public Guid SubscriptionId { get; set; }
        public Subscription? Subcription { get; set; }

        public string? InitialStripeSessionId { get; set; }
        public string? StripeCustomerId { get; set; }

        public Guid? LastActionById { get; set; }
        public User? LastActionBy { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public SubscriptionTransactions()
        {
            CreatedAt = DateTime.Now;
            ModifiedAt = DateTime.Now;
        }
    }
}
