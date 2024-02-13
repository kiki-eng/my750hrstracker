using _750HrsTracker.Models.ActivityLogModels;
using _750HrsTracker.Persistence.Contexts;
using _750HrsTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace _750HrsTracker.Repositories.Implementations
{
    public class ActivityLogCategoryRepository : GenericRepository<ActivityLogCategory>, IActivityLogCategoryRepository
    {  private readonly AppDbContext _context;
        public ActivityLogCategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ActivityLogCategory> UpdateAsync(Guid id, ActivityLogCategory activityLogCategory)
        {
            var existingLogCategory = await _context.ActivityLogCategories.FirstOrDefaultAsync(ala => ala.Id == id) 
                ?? throw new KeyNotFoundException("Log category not found");

            existingLogCategory.Name = activityLogCategory.Name;
            existingLogCategory.AvailablePropertyType = activityLogCategory.AvailablePropertyType;
            existingLogCategory.ModifiedAt = DateTime.Now;

            var updated = _context.ActivityLogCategories.Update(existingLogCategory);
            await _context.SaveChangesAsync();

            return updated.Entity;
        }
    }
}
