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
    }
}
