using _750HrsTracker.Enums;
using _750HrsTracker.Models.ActivityLogModels;
using _750HrsTracker.Persistence.Contexts;
using _750HrsTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace _750HrsTracker.Repositories.Implementations
{
    public class ActivityLogActivityRepository : GenericRepository<ActivityLogActivity>, IActivityLogActivityRepository
    {
        private readonly AppDbContext _context;
        public ActivityLogActivityRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ActivityLogActivity>> GetAllWithTasksAsync(AvailablePropertyType availablePropertyType)
        {
            return await _context.ActivityLogActivities.Include(al => al.ActivityLogCategory).Include(al => al.ActivityLogSubCategories).Where(al => al.AvailablePropertyType == availablePropertyType).ToListAsync();
        }

        public async Task<ActivityLogActivity> UpdateAsync(Guid id, ActivityLogActivity activityLogActivity)
        {
            var existingLogActivity = await _context.ActivityLogActivities.FirstOrDefaultAsync(ala => ala.Id == id) 
                ?? throw new KeyNotFoundException("Log activity not found");

            existingLogActivity.Name = activityLogActivity.Name;
            existingLogActivity.AvailablePropertyType = activityLogActivity.AvailablePropertyType;

            if (activityLogActivity.AvailablePropertyType.Equals(AvailablePropertyType.LTR))
            {
                existingLogActivity.ActivityLogCategoryId = activityLogActivity.ActivityLogCategoryId;  
            }
            existingLogActivity.ModifiedAt = DateTime.Now;

            var updated = _context.ActivityLogActivities.Update(existingLogActivity);
            await _context.SaveChangesAsync();

            return updated.Entity;
        }
    }
}
