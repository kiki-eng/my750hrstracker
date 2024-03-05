using _750HrsTracker.Helpers.Constants;
using _750HrsTracker.Models.ActivityLogModels;
using _750HrsTracker.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace _750HrsTracker.Persistence.Seeds
{
    public static class DefaultLogData
    {
        public static async Task SeedDefaultCatgoriesAsync(AppDbContext context)
        {
            var categories = new List<ActivityLogCategory>
            {
                new ActivityLogCategory()
                {
                    Name = "General real estate activity",
                    Slug = LogCategoryConstants.GeneralRealEstateSlug
                },
                new ActivityLogCategory()
                {
                    Name = "Material Participation",
                    Slug = LogCategoryConstants.MaterialParticipationSlug
                },
            };

            var existingCategories = await context.ActivityLogCategories.ToListAsync();
            foreach (var category in categories)
            {
                if(!existingCategories.Any(ec => ec.Slug == category.Slug))
                {
                    await context.ActivityLogCategories.AddAsync(category);
                    await context.SaveChangesAsync();   
                }
            }
        }
    }
}
