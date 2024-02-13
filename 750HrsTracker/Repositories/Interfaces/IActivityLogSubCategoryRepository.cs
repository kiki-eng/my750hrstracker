using _750HrsTracker.Models.ActivityLogModels;

namespace _750HrsTracker.Repositories.Interfaces
{
    public interface IActivityLogSubCategoryRepository : IGenericRepository<ActivityLogSubCategory>
    {
        Task<ActivityLogSubCategory> UpdateAsync(Guid id,  ActivityLogSubCategory activityLogSubCategory);
    }
}
