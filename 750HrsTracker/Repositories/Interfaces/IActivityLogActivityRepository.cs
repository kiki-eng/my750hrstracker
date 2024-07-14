using _750HrsTracker.Enums;
using _750HrsTracker.Models.ActivityLogModels;

namespace _750HrsTracker.Repositories.Interfaces
{
    public interface IActivityLogActivityRepository : IGenericRepository<ActivityLogActivity>
    {
        Task<ActivityLogActivity> UpdateAsync(Guid id,  ActivityLogActivity activityLogActivity);
        Task<List<ActivityLogActivity>> GetAllWithTasksAsync(AvailablePropertyType availablePropertyType);
    }
}
