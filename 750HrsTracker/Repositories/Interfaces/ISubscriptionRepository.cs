using _750HrsTracker.Models;
using _750HrsTracker.Models.SubscriptionModels;

namespace _750HrsTracker.Repositories.Interfaces
{
    public interface ISubscriptionRepository : IGenericRepository<Subscription>
    {
        Task<Subscription> UpdateAsync(Guid id, Subscription subscription);
        Task<Subscription> GetSubscriptionPermissionsAsync(Guid subscriptionId);
        Task<List<SubscriptionPermission>> UpdateSubscriptionPermissionsAsync(Guid subscriptionId, List<Permission> permissions);
    }
}
