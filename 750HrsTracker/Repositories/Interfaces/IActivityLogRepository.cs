using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Enums;
using _750HrsTracker.Filters;
using _750HrsTracker.Models;
using _750HrsTracker.Models.ActivityLogModels;
using _750HrsTracker.Models.JointEntities;
using _750HrsTracker.Models.ResponseWrappers;

namespace _750HrsTracker.Repositories.Interfaces
{
    public interface IActivityLogRepository : IGenericRepository<ActivityLog>
    {
        Task<ActivityLog> UpdateAsync(Guid id, Guid teamId, ActivityLog property);
        Task<ActivityLogDocument> AttachLogDocumentAsync(ActivityLogDocument activityLogDocument);
        Task AttachLogPropertyAsync(List<ActivityLogProperty> activityLogProperties);
        Task<RepositoryResponseHandler<ActivityLog>> GetAllLogsAsync(Guid teamId, PaginationFilter filter, ActivityLogFilter activityLogFilter, AvailablePropertyType propertyType);

        Task<GetDashboardResponse> GetRecentActivityLogsAsync(Guid teamId, Guid currentUserId, AvailablePropertyType propertyType);

    }
}
