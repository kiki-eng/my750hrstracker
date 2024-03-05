using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Enums;
using _750HrsTracker.Helpers.Constants;
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

        public async Task<GetDashboardResponse> GetRecentActivityLogsAsync(Guid teamId, AvailablePropertyType propertyType)
        {
            GetDashboardResponse response = new();

            var logs = await _context.ActivityLogs.Include(al => al.ActivityLogActivity).Include(al => al.ActivityLogCategory)
                .Where(al => al.PropertyType == propertyType).OrderByDescending(al => al.CreatedAt).ToListAsync();

            if (propertyType.Equals(AvailablePropertyType.LTR))
            {
                logs = logs.Where(l => l.ActivityLogCategory != null && l.ActivityLogCategory!.Slug == LogCategoryConstants.MaterialParticipationSlug).ToList();
            }

            var totalHours = logs.Sum(l => l.HoursSpent);
            var totalMinutes = logs.Sum(l => l.MinutesSpent);
            var totalSeconds = logs.Sum(l => l.HoursSpent);
            var totalTimeInSeconds = (totalHours * 3600) + (totalMinutes * 60) + totalSeconds;

            response.PropertyType = propertyType;
            response.TotalRepsHours = totalTimeInSeconds / 3600;
            response.RecentLogs = logs.Take(5).Select(l => new GetActivityLogResponse()
            {
                Id = l.Id,
                Name = l.Name,
                Category = l.ActivityLogCategory!.Name,
                HoursSpent = l.HoursSpent,
                MinutesSpent = l.MinutesSpent,
                SecondsSpent = l.SecondsSpent,
                ActivityDate = l.ActivityDate,
                ActivityBy = new GetUserResponse() 
                { 
                    FirstName = l.ActivityBy!.Firstname,
                    LastName = l.ActivityBy!.Lastname,
                    Email = l.ActivityBy!.Email
                },
                Description = l.Description,

            }).ToList();

            
            return response;
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
