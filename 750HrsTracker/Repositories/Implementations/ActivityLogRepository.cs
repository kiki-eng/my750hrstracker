using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Enums;
using _750HrsTracker.Helpers.Constants;
using _750HrsTracker.Models.ActivityLogModels;
using _750HrsTracker.Models.JointEntities;
using _750HrsTracker.Persistence.Contexts;
using _750HrsTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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

        public async Task<GetDashboardResponse> GetRecentActivityLogsAsync(Guid teamId, Guid currentUserId, AvailablePropertyType propertyType)
        {
            GetDashboardResponse response = new();

            var logs = await _context.ActivityLogs.Include(al => al.ActivityLogActivity).Include(al => al.ActivityLogCategory)
                .Where(al => al.PropertyType == propertyType && al.TeamId == teamId).OrderByDescending(al => al.CreatedAt).ToListAsync();

            decimal totalHours = 0;
            decimal totalMinutes = 0;
            decimal totalSeconds = 0 ;
            decimal totalTimeInSeconds = 0;

            response.PropertyType = propertyType;
            response.TotalRepsHours = totalTimeInSeconds / 3600;

            if (propertyType.Equals(AvailablePropertyType.LTR))
            {
                var logsGrouped = logs.GroupBy(l => l.LogType);
                List<LogTypeCounts> counts = new List<LogTypeCounts>();

                foreach(var group in logsGrouped)
                {

                   
                    LogTypeCounts logTypeCount = new()
                    {
                            LogType = group.Key.ToString(),

                    };
                
                    if (group.Key == ActivityLogType.REAL_ESTATE)
                    {
                        var categoryGroups = group.GroupBy(l => l.ActivityLogCategoryId);
                            
                        List < GetCategoryHoursCount > categoryHoursCounts = new List<GetCategoryHoursCount>();
                        foreach (var categoryGroup in categoryGroups)
                        {
                            GetCategoryHoursCount getCategoryHoursCount = new GetCategoryHoursCount();
                            getCategoryHoursCount.Name = categoryGroup.First().ActivityLogCategory!.Name;

                            var totalGroupedHours = categoryGroup.Sum(l => l.HoursSpent);
                            var totalGroupedMinutes = categoryGroup.Sum(l => l.MinutesSpent);
                            var totalGroupedSeconds = categoryGroup.Sum(l => l.HoursSpent);
                            var totalGroupedTimeInSeconds = (totalHours * 3600) + (totalMinutes * 60) + totalSeconds;

                            getCategoryHoursCount.Hours = totalTimeInSeconds / 3600;

                            categoryHoursCounts.Add(getCategoryHoursCount);
                        }

                        logTypeCount.Categories = categoryHoursCounts;
                        logTypeCount.TotalHours = categoryHoursCounts.Sum(l => l.Hours);
                    }
                    else if(group.Key == ActivityLogType.NON_REAL_ESTATE)
                    {
                        var totalGroupedHours = group.Sum(l => l.HoursSpent);
                        var totalGroupedMinutes = group.Sum(l => l.MinutesSpent);
                        var totalGroupedSeconds = group.Sum(l => l.HoursSpent);
                        var totalGroupedTimeInSeconds = (totalHours * 3600) + (totalMinutes * 60) + totalSeconds;

                        logTypeCount.TotalHours = totalTimeInSeconds / 3600;
                    }

                    counts.Add(logTypeCount);


                }

                var materialLogs = logs.Where(l => l.ActivityLogCategory != null && l.ActivityLogCategory.Slug == LogCategoryConstants.MaterialParticipationSlug).ToList();
                
                totalHours = materialLogs.Sum(l => l.HoursSpent);
                totalMinutes = materialLogs.Sum(l => l.MinutesSpent);
                totalSeconds = materialLogs.Sum(l => l.HoursSpent);
                totalTimeInSeconds = (totalHours * 3600) + (totalMinutes * 60) + totalSeconds;

                response.TotalRepsHours = totalTimeInSeconds / 3600;
                response.LogHours = counts;
            }
            else
            {
                logs = logs.Where(l => l.ActivityById == currentUserId).ToList();
                totalHours = logs.Sum(l => l.HoursSpent);
                totalMinutes = logs.Sum(l => l.MinutesSpent);
                totalSeconds = logs.Sum(l => l.HoursSpent);
                totalTimeInSeconds = (totalHours * 3600) + (totalMinutes * 60) + totalSeconds;

                response.TotalRepsHours = totalTimeInSeconds / 3600;
            }

            response.PropertyType = propertyType;           
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
