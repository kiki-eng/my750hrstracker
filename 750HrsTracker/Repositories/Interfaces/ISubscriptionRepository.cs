using _750HrsTracker.Enums;
using _750HrsTracker.Models;
using _750HrsTracker.Models.SubscriptionModels;

namespace _750HrsTracker.Repositories.Interfaces
{
    public interface ISubscriptionRepository : IGenericRepository<Subscription>
    {
        Task<Subscription> UpdateAsync(Guid id, Subscription subscription);
        Task<Subscription> UpdatePriceIdAsync(Guid id, Subscription subscription);
        Task<Subscription> UpdateFeaturesAsync(Guid id, List<string> features);
        Task<Subscription> GetSubscriptionPermissionsAsync(Guid subscriptionId);
        Task<Subscription> GetSubscriptionByPriceIdAsync(string priceId, SubscriptionType subscriptionType = SubscriptionType.stripe);
        Task<SubscriptionTransactions> GetSubscriptionTransactionBySessionIdAsync(string checkoutSessionId);
        Task<SubscriptionTransactions> GetSubscriptionTransactionByStripeRecIdAsync(string recId, string filter = "");
        Task<SubscriptionTransactions> UpdateSubscriptionTransactionAsync(Guid id, SubscriptionTransactions subscriptionTransactions, 
            SubscriptionTransactionUpdateAction updateAction = SubscriptionTransactionUpdateAction.none);
        Task<List<SubscriptionPermission>> UpdateSubscriptionPermissionsAsync(Guid subscriptionId, List<Permission> permissions);
    }
}
