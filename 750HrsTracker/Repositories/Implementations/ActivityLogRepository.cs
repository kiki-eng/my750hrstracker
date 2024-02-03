using _750HrsTracker.Models;
using _750HrsTracker.Models.ActivityLogModels;
using _750HrsTracker.Models.JointEntities;
using _750HrsTracker.Persistence.Contexts;
using _750HrsTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace _750HrsTracker.Repositories.Implementations
{
    public class ActivityLogRepository : GenericRepository<ActivityLog>, IActivityLogRepository
    {
        private readonly AppDbContext _context;
        public ActivityLogRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ActivityLogDocument> AttachLogDocumentAsync(ActivityLogDocument activityLogDocument)
        {
            var added = await _context.ActivityLogDocuments.AddAsync(activityLogDocument);  
            await _context.SaveChangesAsync();

            return added.Entity;
        }

        public async Task AttachLogPropertyAsync(List<ActivityLogProperty> activityLogProperties)
        {
            await _context.ActivityLogProperties.AddRangeAsync(activityLogProperties);
            await _context.SaveChangesAsync();
        }

        public async Task<ActivityLog> UpdateAsync(Guid id, Guid teamId, ActivityLog activityLog)
        {
            var existingActivityLog = await _context.ActivityLogs.FirstOrDefaultAsync(p => p.Id == id && p.TeamId == teamId) ?? throw new KeyNotFoundException("Activity log not found");

            existingActivityLog.ActivityDate = activityLog.ActivityDate;
            existingActivityLog.HoursSpent = activityLog.HoursSpent;
            existingActivityLog.MinutesSpent = activityLog.MinutesSpent;
            existingActivityLog.Description = activityLog.Description;
            existingActivityLog.ModifiedAt = DateTime.Now;

            var updated = _context.ActivityLogs.Update(existingActivityLog);

            await _context.SaveChangesAsync();

            return updated.Entity;

        }
    }
}
