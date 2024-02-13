using _750HrsTracker.Models.ActivityLogModels;

namespace _750HrsTracker.Repositories.Interfaces
{
    public interface IActivityLogCategoryRepository : IGenericRepository<ActivityLogCategory>
    {
        Task<ActivityLogCategory> UpdateAsync(Guid id,  ActivityLogCategory activityLogCategory);
    }
}
