using _750HrsTracker.Enums;
using _750HrsTracker.Helpers;
using _750HrsTracker.Models.SubscriptionModels;
using _750HrsTracker.Persistence.Contexts;
using _750HrsTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace _750HrsTracker.Repositories.Implementations
{
    public class TeamSubscriptionRepository : GenericRepository<TeamSubscription>, ITeamSubscriptionRepository
    {
        private readonly AppDbContext _context;
        public TeamSubscriptionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<TeamSubscription> FindPermissionInTeamSubscriptionAsync(Guid teamId, string permissionValue)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == teamId) ?? throw new KeyNotFoundException("Could not identify account");

            var teamSubscription = await _context.TeamSubscriptions.FirstOrDefaultAsync(t => t.TeamId == teamId) ?? throw new KeyNotFoundException("No subscription available");

            var permission = await _context.Permissions.Include(p => p.SubscriptionPermissions).FirstOrDefaultAsync(p => p.Value == permissionValue) ?? throw new KeyNotFoundException("Permission not found");

            if(permission.SubscriptionPermissions == null || !permission.SubscriptionPermissions.Any(ps => ps.SubscriptionId == teamSubscription.SubscriptionId))
            {
                throw new ForbiddenAccessException("Subscription does not have permission to this resource");
            }

            return teamSubscription;
        }

        public async Task<TeamSubscription> GetWthSubscription(Guid teamId)
        {
            var teamSub = await _context.TeamSubscriptions.Include(ts => ts.Subscription).OrderByDescending(ts => ts.CreatedAt).FirstOrDefaultAsync(ts => ts.TeamId == teamId && !ts.Canceled && ts.IsActive);

            return teamSub!;
        }

        public async Task<TeamSubscription> UpdateTeamSubscriptionAsync(TeamSubscription teamSubscription, TeamSubscriptionUpdateAction updateAction = TeamSubscriptionUpdateAction.none)
        {
            var existingTeamSubscription = await _context.TeamSubscriptions.FirstOrDefaultAsync(t => t.TeamId == teamSubscription.TeamId 
                && t.SubscriptionId == teamSubscription.SubscriptionId && t.SubscriptionTransactionId == teamSubscription.SubscriptionTransactionId) 
                ?? throw new KeyNotFoundException("No subscription available");

            switch (updateAction)
            {
                case TeamSubscriptionUpdateAction.none:
                    existingTeamSubscription.SubscriptionTransactionId = teamSubscription.SubscriptionTransactionId;
                    existingTeamSubscription.StripeSubscriptionId = teamSubscription.StripeSubscriptionId;
                    existingTeamSubscription.StartDate = teamSubscription.StartDate;
                    existingTeamSubscription.EndDate = teamSubscription.EndDate;
                    existingTeamSubscription.ModifiedAt = DateTime.Now;
                    existingTeamSubscription.IsActive = teamSubscription.IsActive;
                    break;
                case TeamSubscriptionUpdateAction.cancel:
                    existingTeamSubscription.Canceled = true;
                    existingTeamSubscription.IsActive = false;
                    existingTeamSubscription.CanceledAt = DateTime.Now;
                    break;

                default:
                    break;
            }
            var updated = _context.TeamSubscriptions.Update(existingTeamSubscription);

            await _context.SaveChangesAsync();

            return updated.Entity;

        }

    }
}
