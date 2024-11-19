using _750HrsTracker.Helpers;
using _750HrsTracker.Helpers.Constants;
using _750HrsTracker.Models.Admin;
using _750HrsTracker.Models.SubscriptionModels;
using _750HrsTracker.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace _750HrsTracker.Persistence.Seeds
{
    public static class DefaultSubscriptions
    {
        public static async Task SeedAsync(AppDbContext context, AppSettings _appSettings)
        {
            var subscriptions = new List<Subscription>()
            {
                new Subscription
                {
                    Name = "Monthly Plan",
                    CreatedAt = DateTime.Now,
                    Price = 19.99m,
                    Slug = "monthly-plan",
                    SubscriptionInterval = Enums.SubscriptionInterval.monthly,
                    Features = JsonConvert.SerializeObject(new List<string>()
                    {
                        "All features made available",
                        "Third party will be able to access or download your files",
                        "You will be able to upload and store supporting documents",
                    })
                },
                new Subscription
                {
                    Name = "Yearly Plan",
                    CreatedAt = DateTime.Now,
                    Price = 199.99m,
                    Slug = "yearly-plan",
                    SubscriptionInterval = Enums.SubscriptionInterval.yearly,
                    Features = JsonConvert.SerializeObject(new List<string>()
                    {
                        "All features made available",
                        "Third party will be able to access or download your files",
                        "You will be able to upload and store supporting documents",
                        "$40 discount"
                    })
                },               
                new Subscription
                {
                    Name = "Free Plan",
                    CreatedAt = DateTime.Now,
                    Price = 0,
                    Slug = SubscriptionConstants.FreeplanSlug,
                    SubscriptionInterval = Enums.SubscriptionInterval.none,
                    Features = JsonConvert.SerializeObject(new List<string>()
                    {
                        "All features made available for the first 30 days",
                        "Third party won’t be able to access or download your files",
                        "You won’t be able to upload supporting documents"
                    })
                }
            };

            var existingSubscriptions = await context.Subscriptions.ToListAsync();
            foreach (var subscription in subscriptions)
            {
                if (!existingSubscriptions.Any(a => a.Slug == subscription.Slug))
                {
                    await context.Subscriptions.AddAsync(subscription);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
