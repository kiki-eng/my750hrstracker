using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Enums;
using _750HrsTracker.Filters;
using _750HrsTracker.Helpers.Constants;
using _750HrsTracker.Models;
using _750HrsTracker.Models.ActivityLogModels;
using _750HrsTracker.Models.JointEntities;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Persistence.Contexts;
using _750HrsTracker.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace _750HrsTracker.Repositories.Implementations
{
    public class ActivityLogRepository : GenericRepository<ActivityLog>, IActivityLogRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ActivityLogRepository(AppDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ActivityLogDocument> AttachLogDocumentAsync(ActivityLogDocument activityLogDocument)
        {
            var added = await _context.ActivityLogDocuments.AddAsync(activityLogDocument);  
            await _context.SaveChangesAsync();

            return added.Entity;
        }
        
        public async Task DetachLogDocumentAsync(Guid activityLogId)
        {
            var documents = await _context.ActivityLogDocuments.Where(ad => ad.ActivityLogId == activityLogId).ToListAsync();

            _context.ActivityLogDocuments.RemoveRange(documents);

            await _context.SaveChangesAsync();
        }

        public async Task AttachLogPropertyAsync(List<ActivityLogProperty> activityLogProperties)
        {
            await _context.ActivityLogProperties.AddRangeAsync(activityLogProperties);
            await _context.SaveChangesAsync();
        }

        public async Task<GetDashboardResponse> GetRecentActivityLogsAsync(Guid teamId, Guid currentUserId, AvailablePropertyType propertyType)
        {
            GetDashboardResponse response = new();

            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == teamId) ?? throw new ApplicationException("Team not found") ;
            var adminUsers = _context.Users.ToList().Where(u => u.Id == (Guid)team.OwnerId! || u.IsOwnerSpouse ).ToList();

            var logs = await _context.ActivityLogs.Include(al => al.ActivityLogActivity).ThenInclude(al => al!.ActivityLogCategory)
                .Include(al => al.Task)
                .Include(al => al.ActivityLogProperties)!.ThenInclude(al => al.Property)
                .Where(al => al.PropertyType == propertyType && al.TeamId == teamId).OrderByDescending(al => al.CreatedAt).ToListAsync();

            decimal totalHours = 0;
            decimal totalMinutes = 0;
            decimal totalSeconds = 0 ;
            decimal totalTimeInSeconds = 0;

            response.TeamMembersCount = await _context.Team_User.Where(tu => tu.TeamId == teamId).CountAsync();

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
                        List<GetCategoryHoursCount> categoryHoursCounts = new List<GetCategoryHoursCount>();
                        var categories = await _context.ActivityLogCategories.ToListAsync();
                        foreach (var category in categories)
                        {
                            var cat = logs.Where(l => l.LogType == ActivityLogType.REAL_ESTATE && l.ActivityLogCategoryId == category.Id).ToList();
                            GetCategoryHoursCount getCategoryHoursCount = new()
                            {
                                Name = category.Name,
                                Id = category.Id,
                                Slug = category.Slug,
                            };

                            if (cat.Count > 0)
                            {
                                var totalGroupedHours = cat.Sum(l => l.HoursSpent);
                                var totalGroupedMinutes = cat.Sum(l => l.MinutesSpent);
                                var totalGroupedSeconds = cat.Sum(l => l.HoursSpent);
                                var totalGroupedTimeInSeconds = (totalGroupedHours * 3600) + (totalGroupedMinutes * 60) + totalGroupedSeconds;

                                decimal totalGroupedRepHours = totalGroupedTimeInSeconds / 3600;
                                getCategoryHoursCount.Hours = Math.Round(totalGroupedRepHours, 2);
                            }

                            getCategoryHoursCount.UserHours = GetUserHoursAsync(team, adminUsers, cat);

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
                        var totalGroupedTimeInSeconds = (totalGroupedHours * 3600) + (totalGroupedMinutes * 60) + totalGroupedSeconds;

                        decimal totalRepHours = totalGroupedTimeInSeconds / 3600;

                        logTypeCount.TotalHours = Math.Round(totalRepHours, 2);
                    }

                    counts.Add(logTypeCount);


                }
                
                if(!counts.Any(lg => lg.LogType!.Equals(ActivityLogType.REAL_ESTATE.ToString())))
                {
                    LogTypeCounts realEstateCount = new()
                    {
                        LogType = ActivityLogType.REAL_ESTATE.ToString(),                       
                        TotalHours = 0,
                    };
                    var logCategories = await _context.ActivityLogCategories.ToListAsync();

                    realEstateCount.Categories = logCategories.Select(lg => new GetCategoryHoursCount
                    {
                        Id = lg.Id,
                        Name = lg.Name,
                        Slug = lg.Slug,
                        Hours = 0,
                        UserHours = GetUserHoursAsync(team, adminUsers, new List<ActivityLog>())
                    }).ToList();                        
                    
                    counts.Add(realEstateCount);
                }
                
                if(!counts.Any(lg => lg.LogType!.Equals(ActivityLogType.NON_REAL_ESTATE.ToString())))
                {
                    LogTypeCounts nonRealEstateCount = new()
                    {
                        LogType = ActivityLogType.NON_REAL_ESTATE.ToString(),
                        TotalHours = 0,
                    };

                    counts.Add(nonRealEstateCount);
                }
                var materialLogs = logs.Where(l => l.ActivityLogCategory != null && l.ActivityLogCategory.Slug == LogCategoryConstants.MaterialParticipationSlug).ToList();

               
                
                
                totalHours = materialLogs.Sum(l => l.HoursSpent);
                totalMinutes = materialLogs.Sum(l => l.MinutesSpent);
                totalSeconds = materialLogs.Sum(l => l.SecondsSpent);
                totalTimeInSeconds = (totalHours * 3600) + (totalMinutes * 60) + totalSeconds;

                response.TotalRepsHours = Math.Round((totalTimeInSeconds / 3600), 2);
                response.LogHours = counts;

            }
            else
            {
                var userLogs = logs.Where(l => l.ActivityById == currentUserId).ToList();
                totalHours = userLogs.Sum(l => l.HoursSpent);
                totalMinutes = userLogs.Sum(l => l.MinutesSpent);
                totalSeconds = userLogs.Sum(l => l.HoursSpent);
                totalTimeInSeconds = (totalHours * 3600) + (totalMinutes * 60) + totalSeconds;


                var users = await _context.Team_User.Include(tu => tu.User).Where(tu => tu.TeamId == team.Id).ToListAsync();

                response.TotalRepsHours = Math.Round((totalTimeInSeconds / 3600), 2);
                response.UserHours = GetUserHoursAsync(team, users.Select(u => new User
                {
                    Id = (Guid)u.UserId!,
                    Firstname = u.User!.Firstname,
                    Lastname = u.User!.Lastname,
                    IsOwnerSpouse = u.User.IsOwnerSpouse
                }).ToList(), logs);
            }

            response.PropertyType = propertyType;

            var latestFive = logs.Take(5).ToList();
            
            response.RecentLogs = logs.Take(5).Select(l => MappedResponse(l)).ToList();
            
            return response;
        }

        private GetActivityLogResponse MappedResponse(ActivityLog activityLog)
        {
            var response = _mapper.Map<GetActivityLogResponse>(activityLog);

            if (activityLog.ActivityLogActivity != null)
            {
                response.Activity = new GetActivityLogActivityResponse
                {
                    Name = activityLog.ActivityLogActivity.Name,
                    Id = activityLog.ActivityLogActivity.Id
                };

                //response.Activity = activityLog.ActivityLogActivity.Name;
            }

            if (activityLog.Task != null)
            {
                response.Task = new GetLogActivitySubCategoryResponse()
                {
                    Id = activityLog.Task.Id,
                    Name = activityLog.Task.Name,
                    Slug = activityLog.Task.Slug,
                };
            }

            if (activityLog.ActivityLogActivity != null && activityLog.ActivityLogActivity.ActivityLogCategory != null)
            {
                response.Category = new GetActivityLogCategoryResponse
                {
                    Name = activityLog.ActivityLogActivity.ActivityLogCategory.Name,
                    Id = activityLog.ActivityLogActivity.ActivityLogCategory.Id,
                };

                //response.Category = activityLog.ActivityLogActivity.ActivityLogCategory.Name;
            }

            if (activityLog.ActivityLogProperties != null && activityLog.ActivityLogProperties.Count > 0)
            {
                List<GetPropertyResponse> properties = new List<GetPropertyResponse>();

                foreach (var property in activityLog.ActivityLogProperties)
                {
                    GetPropertyResponse propertyResponse = new()
                    {
                        Name = property.Property!.Name,
                        Description = property.Property!.Description,
                        Id = property.Property!.Id,
                    };

                    properties.Add(propertyResponse);
                }

                response.Properties = properties;
            }

            return response;
        }
        private List<UserHoursModel> GetUserHoursAsync(Team team, List<User> adminUsers, List<ActivityLog> logs)
        {
            List<UserHoursModel> userHours = new();
            foreach (var user in adminUsers)
            {
                UserHoursModel model = new UserHoursModel
                {
                    Id = user.Id,
                    Name = $"{user.Firstname} {user.Lastname}",
                    IsSpouse = user.IsOwnerSpouse,
                    IsAdmin = user.Id == team.OwnerId
                };
                var userLogs = logs.Where(ml => ml.ActivityById == user.Id).ToList();
                var totalHours = userLogs.Sum(l => l.HoursSpent);
                var totalMinutes = userLogs.Sum(l => l.MinutesSpent);
                var totalSeconds = userLogs.Sum(l => l.SecondsSpent);
                var totalTimeInSeconds = (totalHours * 3600) + (totalMinutes * 60) + totalSeconds;

                model.Hours = totalTimeInSeconds / 3600;

                userHours.Add(model);
            }

            return userHours;
        }

        public async Task<ActivityLog> UpdateAsync(Guid id, Guid teamId, ActivityLog activityLog)
        {
            var existingActivityLog = await _context.ActivityLogs.FirstOrDefaultAsync(p => p.Id == id && p.TeamId == teamId) ?? throw new KeyNotFoundException("Activity log not found");

            existingActivityLog.Name = activityLog.Name;
            existingActivityLog.ActivityById = activityLog.ActivityById;

            if (activityLog.LogType != ActivityLogType.NON_REAL_ESTATE)
            {
                if (activityLog.LogType == ActivityLogType.REAL_ESTATE)
                {
                    existingActivityLog.TaskId = activityLog.TaskId;
                    existingActivityLog.ActivityLogCategoryId = activityLog.ActivityLogCategoryId;

                }
                existingActivityLog.ActivityLogActivityId = activityLog.ActivityLogActivityId;

            }
            existingActivityLog.ActivityDate = activityLog.ActivityDate;
            existingActivityLog.HoursSpent = activityLog.HoursSpent;
            existingActivityLog.MinutesSpent = activityLog.MinutesSpent;
            existingActivityLog.SecondsSpent = activityLog.SecondsSpent;
            existingActivityLog.Description = activityLog.Description;
            existingActivityLog.LogType = activityLog.LogType;
            existingActivityLog.ModifiedAt = DateTime.Now;

            var updated = _context.ActivityLogs.Update(existingActivityLog);

            await _context.SaveChangesAsync();

            return updated.Entity;

        }

        public async Task<RepositoryResponseHandler<ActivityLog>> GetAllLogsAsync(PaginationFilter filter, ActivityLogFilter activityLogFilter, 
            AvailablePropertyType propertyType, Guid? teamId = null, bool withIncludes = false)
        {

                
            IQueryable<ActivityLog> query = _context.ActivityLogs.OrderByDescending(al => al.CreatedAt);

            if (teamId is not null)
            {
                query = query.Where(al => al.TeamId == teamId);
            }

            if(propertyType == AvailablePropertyType.STR)
            {
                query = query.Where(al => al.PropertyType == propertyType);
            }
            
            if(propertyType == AvailablePropertyType.LTR)
            {
                query = query.Where(al => al.PropertyType == propertyType);
            }

            if (activityLogFilter.Activity != Guid.Empty)
            {
               query = query.Where(al => al.ActivityLogActivityId == activityLogFilter.Activity);
            }
            
            if(activityLogFilter.Member != Guid.Empty)
            {
                query = query.Where(al => al.ActivityById == activityLogFilter.Member);
            }
            
            if(activityLogFilter.Property != Guid.Empty)
            {
                query = query.Where(al => al.ActivityLogProperties!.Any(alp => alp.PropertyId == activityLogFilter.Property));
            }
            else
            {
                query = query.Include(al => al.ActivityLogProperties);
            }

            if (activityLogFilter.WithDocuments)
            {
                if (activityLogFilter.AllSupportingDocument)
                {
                    query = query.Include(al => al.ActivityLogDocuments);
                }
                else
                {
                    if (activityLogFilter.HasSupportingDocument)
                    {
                        query = query.Include(al => al.ActivityLogDocuments).Where(al => al.ActivityLogDocuments != null && al.ActivityLogDocuments.Count > 0);
                    }
                    else 
                    {
                        query = query.Include(al => al.ActivityLogDocuments).Where(al => al.ActivityLogDocuments == null);
                    }
                }
            }
            if (activityLogFilter.StartDate != null || activityLogFilter.EndDate != null)
            {

                var startDate = activityLogFilter.StartDate == null ? new DateTime(1970, 1, 1).Add(new TimeSpan(0, 0, 0)) : (DateTime)activityLogFilter.StartDate;

                var today = DateTime.Today;
                var endDate = activityLogFilter.EndDate == null ? new DateTime(today.Year, today.Month, today.Day).Add(new TimeSpan(23,59,59)) : (DateTime)activityLogFilter.EndDate;

                activityLogFilter.StartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day).Add(new TimeSpan(0, 0, 0));
                activityLogFilter.EndDate = new DateTime(endDate.Year, endDate.Month, endDate.Day).Add(new TimeSpan(23, 59, 59));

                query = query.Where(al => al.CreatedAt >= startDate && al.CreatedAt <= endDate);
            }

            if(filter.PageSize > 0)
            {
                query = query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);
            }
            var records = await query
                .Include(al => al.ActivityBy)
                .Include(al => al.ActivityLogActivity).ThenInclude(la => la!.ActivityLogCategory)
                .Include(al => al.ActivityLogProperties!).ThenInclude(la => la!.Property)
                .Include(al => al.Task)
                .ToListAsync();

            var totalCount = await query.CountAsync();

            RepositoryResponseHandler<ActivityLog> response = new RepositoryResponseHandler<ActivityLog>
            {
                Records = records,
                TotalCount = totalCount
            };

            return response;
        }

        public async Task<List<ActivityLogDocument>> GetDocumentsAsync(Guid logId)
        {
            return await _context.ActivityLogDocuments.Where(ald => ald.ActivityLogId == logId).ToListAsync();
        }
        
        public async Task<List<ActivityLogDocument>> GetDocumentsByTeamIdAsync(Guid teamId, ExportDocumentFilter filter)
        {
            IQueryable<ActivityLogDocument> query = _context.ActivityLogDocuments.Where(ald => ald.TeamId == teamId);

            if(filter.year != null)
            {
                query = query.Where(q => q.CreatedAt.Year == Convert.ToInt32(filter.year));
            }
            
            return await query.ToListAsync();
        }

        public async Task UpdateAttachedLogDocumentAsync(Guid activityLogId, List<ActivityLogDocument> activityLogDocuments)
        {
            var existingDocuments = await _context.ActivityLogDocuments.Where(ald => ald.ActivityLogId == activityLogId).ToListAsync();

            if(existingDocuments.Count > 0)
            {
                _context.ActivityLogDocuments.RemoveRange(existingDocuments);
            }

            await _context.ActivityLogDocuments.AddRangeAsync(activityLogDocuments);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAttachedLogPropertyAsync(Guid activityLogId, List<ActivityLogProperty> activityLogProperties)
        {
            var existingProperties = await _context.ActivityLogProperties.Where(alp => alp.ActivityLogId.Equals(activityLogId)).ToListAsync();  

            if(existingProperties.Count > 0)
            {
                _context.ActivityLogProperties.RemoveRange(existingProperties);
            }

            await _context.ActivityLogProperties.AddRangeAsync(activityLogProperties);

            await _context.SaveChangesAsync();
        }

        public async Task<ActivityLog> GetLogByIdAsync(Guid id, Guid? teamId = null)
        {
            IQueryable<ActivityLog> query = _context.ActivityLogs;

            if(teamId is not null)
            {
                query = query.Where(al => al.TeamId == teamId);
            }
            var log = await query.Include(al => al.ActivityLogActivity).ThenInclude(al => al!.ActivityLogCategory)
                .Include(al => al.Task)
                .Include(al => al.ActivityLogProperties)!.ThenInclude(al => al.Property)
                .Include(al => al.ActivityBy)
                .Include(al => al.Team)
                .FirstOrDefaultAsync(l => l.Id == id) ?? throw new KeyNotFoundException("Log not found");

            return log;
        }
    }
}
