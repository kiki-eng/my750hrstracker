using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Models.SubscriptionModels;

namespace _750HrsTracker.Repositories.Interfaces
{
    public interface IWebhookNotificationRepository : IGenericRepository<WebhookNotificationTraceLog>
    {
        Task<WebhookNotificationTraceLog> UpdateAsync(long id, WebhookNotificationTraceLog webhookNotificationTraceLog);
    }
}
