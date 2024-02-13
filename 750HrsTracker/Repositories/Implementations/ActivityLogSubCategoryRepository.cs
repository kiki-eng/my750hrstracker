using _750HrsTracker.Enums;
using _750HrsTracker.Models.ActivityLogModels;
using _750HrsTracker.Persistence.Contexts;
using _750HrsTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace _750HrsTracker.Repositories.Implementations
{
    public class ActivityLogSubCategoryRepository : GenericRepository<ActivityLogSubCategory>, IActivityLogSubCategoryRepository
    {
        private readonly AppDbContext _context;
        public ActivityLogSubCategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ActivityLogSubCategory> UpdateAsync(Guid id, ActivityLogSubCategory activityLogSubCategory)
        {
            var existingLogSubCategory = await _context.ActivityLogSubCategories.FirstOrDefaultAsync(ala => ala.Id == id) 
                ?? throw new KeyNotFoundException("Log activity sub category not found");

            existingLogSubCategory.Name = activityLogSubCategory.Name;
            existingLogSubCategory.Slug = activityLogSubCategory.Slug;         
            existingLogSubCategory.ModifiedAt = DateTime.Now;

            var updated = _context.ActivityLogSubCategories.Update(existingLogSubCategory);
            await _context.SaveChangesAsync();

            return updated.Entity;
        }
    }
}
