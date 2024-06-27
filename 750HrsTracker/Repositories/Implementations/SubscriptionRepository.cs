using _750HrsTracker.Helpers;
using _750HrsTracker.Models;
using _750HrsTracker.Models.SubscriptionModels;
using _750HrsTracker.Persistence.Contexts;
using _750HrsTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
            existingSubscription.SubscriptionInterval = subscription.SubscriptionInterval;
            existingSubscription.ModifiedAt = DateTime.Now;

            var updated = _context.Subscriptions.Update(existingSubscription);

            await _context.SaveChangesAsync();

            return updated.Entity;
        }
        
        public async Task<Subscription> UpdatePriceIdAsync(Guid id, Subscription subscription)
        {
            var existingSubscription = await _context.Subscriptions.FirstOrDefaultAsync(p => p.Id == id) ?? throw new KeyNotFoundException("Subscription not found");

            existingSubscription.StripePriceId = subscription.StripePriceId;
            existingSubscription.ModifiedAt = DateTime.Now;

            var updated = _context.Subscriptions.Update(existingSubscription);

            await _context.SaveChangesAsync();

            return updated.Entity;
        }

        public async Task<Subscription> GetSubscriptionPermissionsAsync(Guid subscriptionId)
        {
            var subscription = await _context.Subscriptions.Include(s => s.SubcriptionPermissions!).ThenInclude(sp => sp.Permission).FirstOrDefaultAsync(t => t.Id == subscriptionId) 
                ?? throw new KeyNotFoundException("No subscription available");

            return subscription;
        }

        public async Task<List<SubscriptionPermission>> UpdateSubscriptionPermissionsAsync(Guid subscriptionId, List<Permission> permissions)
        {
            List<SubscriptionPermission> updatedPermissions = new List<SubscriptionPermission>();   

            var subscription = await _context.Subscriptions.Include(s => s.SubcriptionPermissions).FirstOrDefaultAsync(s => s.Id == subscriptionId) ?? throw new KeyNotFoundException("Subscription not found");

            var allPermissions = await _context.Permissions.ToListAsync();

            //check if all permssions to update exists
            if (!allPermissions.Any(ap => permissions.Any(p => p.Id == ap.Id)))
            {
                throw new KeyNotFoundException("One or more permissions to update for subscription does not exist");
            }

            if(subscription.SubcriptionPermissions != null && subscription.SubcriptionPermissions.Count > 0)
            {
                _context.SubscriptionPermissions.RemoveRange(subscription.SubcriptionPermissions);
            }

            foreach(var permission in permissions)
            {
                SubscriptionPermission subscriptionPermission = new()
                {
                    SubscriptionId = subscription.Id,
                    PermissionId = permission.Id,
                };

                updatedPermissions.Add(subscriptionPermission);
            }

            await _context.SubscriptionPermissions.AddRangeAsync(updatedPermissions);

            await _context.SaveChangesAsync();  

            return await _context.SubscriptionPermissions.Where(sp => sp.SubscriptionId == subscription.Id).ToListAsync();
        }

        public async Task<Subscription> UpdateFeaturesAsync(Guid id, List<string> features)
        {
            var subscription = await _context.Subscriptions.FirstOrDefaultAsync(t => t.Id == id)
               ?? throw new KeyNotFoundException("Subscription not found");

            subscription.Features = JsonConvert.SerializeObject(features);

            var updated = _context.Subscriptions.Update(subscription);

            await _context.SaveChangesAsync();  

            return updated.Entity;
        }

        public async Task<Subscription> GetSubscriptionByPriceIdAsync(string priceId)
        {
            var subscription = await _context.Subscriptions.FirstOrDefaultAsync(t => t.StripePriceId == priceId)
                ?? throw new KeyNotFoundException("No subscription available");

            return subscription;
        }
    }
}
