using System.ComponentModel.DataAnnotations;

namespace _750HrsTracker.Models.SubscriptionModels
{
    public class WebhookNotificationTraceLog
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(200)]
        public string? EventType { get; set; }
        public string? RequestData { get; set; }
        public string? ResponseData { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public WebhookNotificationTraceLog()
        {
            CreatedAt = DateTime.Now;
            ModifiedAt = DateTime.Now;
        }

    }
}
