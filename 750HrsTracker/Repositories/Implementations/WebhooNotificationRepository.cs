using _750HrsTracker.Models.SubscriptionModels;
using _750HrsTracker.Persistence.Contexts;
using _750HrsTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace _750HrsTracker.Repositories.Implementations
{
    public class WebhooNotificationRepository : GenericRepository<WebhookNotificationTraceLog>, IWebhookNotificationRepository
    {
        private readonly AppDbContext _context;
        public WebhooNotificationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<WebhookNotificationTraceLog> UpdateAsync(long id, WebhookNotificationTraceLog webhookNotificationTraceLog)
        {
            var existingLog = await _context.WebhookNotificationTraceLogs.FirstOrDefaultAsync(w => w.Id == id) ?? throw new KeyNotFoundException("Webhook notification not found");

            existingLog.ResponseData = webhookNotificationTraceLog.ResponseData;
            existingLog.ModifiedAt = DateTime.Now;

            var updated = _context.WebhookNotificationTraceLogs.Update(existingLog);
            await _context.SaveChangesAsync();

            return updated.Entity;
        }
    }
}
