using _750HrsTracker.Models.SubscriptionModels;
using _750HrsTracker.Persistence.Contexts;
using _750HrsTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace _750HrsTracker.Repositories.Implementations
{
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        private readonly AppDbContext _context;
        public SubscriptionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Subscription> UpdateAsync(Guid id, Subscription subscription)
        {
            var existingSubscription = await _context.Subscriptions.FirstOrDefaultAsync(p => p.Id == id) ?? throw new KeyNotFoundException("Subscription not found");

            existingSubscription.Name = subscription.Name;
            existingSubscription.Slug = subscription.Slug;
            existingSubscription.GracePeriodMinutes = subscription.GracePeriodMinutes;
            existingSubscription.ModifiedAt = DateTime.Now;

            var updated = _context.Subscriptions.Update(existingSubscription);

            await _context.SaveChangesAsync();

            return updated.Entity;
        }
    }
}
