using _750HrsTracker.Models.SubscriptionModels;

namespace _750HrsTracker.Repositories.Interfaces
{
    public interface ITeamSubscriptionRepository : IGenericRepository<TeamSubscription>
    {
        Task<TeamSubscription> FindPermissionInTeamSubscriptionAsync(Guid teamId, string permission);
        Task<TeamSubscription> UpdateTeamSubscriptionAsync(TeamSubscription teamSubscription);
    }
}
