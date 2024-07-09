using _750HrsTracker.Enums;
using _750HrsTracker.Helpers;
using _750HrsTracker.Models;
using _750HrsTracker.Models.SubscriptionModels;
using _750HrsTracker.Persistence.Contexts;
using _750HrsTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace _750HrsTracker.Repositories.Implementations
{
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        private readonly AppDbContext _context;
        private readonly Bugsnag.IClient _bugsnag;
        public SubscriptionRepository(AppDbContext context, Bugsnag.IClient bugsnag) : base(context)
        {
            _context = context;
            _bugsnag = bugsnag;
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

        public async Task<SubscriptionTransactions> GetSubscriptionTransactionBySessionIdAsync(string checkoutSessionId)
        {
            var subTransaction = await _context.SubscriptionTransactions.FirstOrDefaultAsync(s => s.InitialStripeSessionId == checkoutSessionId);

            return subTransaction!;
        }
        public async Task<SubscriptionTransactions> GetSubscriptionTransactionByStripeRecIdAsync(string recId, string filter = "")
        {
            SubscriptionTransactions? subscriptionTransaction = filter switch
            {
                "invoice" => await _context.SubscriptionTransactions.FirstOrDefaultAsync(st => st.StripeInvoiceId == recId),
                "subscription" => await _context.SubscriptionTransactions.FirstOrDefaultAsync(st => st.StripeSubscriptionId == recId),
                "customer" => await _context.SubscriptionTransactions.FirstOrDefaultAsync(st => st.StripeCustomerId == recId),
                "session" => await _context.SubscriptionTransactions.FirstOrDefaultAsync(s => s.InitialStripeSessionId == recId),
                _ => null,
            };
            return subscriptionTransaction!;
        }

        public async Task<SubscriptionTransactions> UpdateSubscriptionTransactionAsync(Guid id, SubscriptionTransactions subscriptionTransaction, SubscriptionTransactionUpdateAction updateAction = SubscriptionTransactionUpdateAction.none)
        {
            try
            {
                var subTransaction = await _context.SubscriptionTransactions.FirstOrDefaultAsync(st => st.Id == id) 
                    ?? throw new KeyNotFoundException("Could not find subscription transaction");

                switch(updateAction)
                {
                    case SubscriptionTransactionUpdateAction.checkout_session_completed:
                        subTransaction.StripeSubscriptionId = subscriptionTransaction.StripeSubscriptionId;
                        subTransaction.StripeCustomerId = subscriptionTransaction.StripeCustomerId;
                        subTransaction.StripeInvoiceId = subscriptionTransaction.StripeInvoiceId;
                        subTransaction.StripeEventId = subscriptionTransaction.StripeEventId;
                        subTransaction.StripeEventName = subscriptionTransaction.StripeEventName;
                        subTransaction.EventDataObject = subscriptionTransaction.EventDataObject;
                        break;
                    case SubscriptionTransactionUpdateAction.invoice_paid:
                        break;
                    default:
                        throw new ApplicationException("Invalid subscription transaction update action");
                }

                subscriptionTransaction.IsCheckoutTransaction = subscriptionTransaction.IsCheckoutTransaction;

                var updated = _context.SubscriptionTransactions.Update(subTransaction);
                await _context.SaveChangesAsync();

                return updated.Entity;

            }catch(Exception ex)
            {
                _bugsnag.Notify(ex);
                throw;
            }
        }
    }
}
