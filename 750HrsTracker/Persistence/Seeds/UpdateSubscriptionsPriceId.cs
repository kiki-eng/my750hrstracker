using _750HrsTracker.Enums;
using _750HrsTracker.Helpers;
using _750HrsTracker.Helpers.Constants;
using _750HrsTracker.Models.ActivityLogModels;
using _750HrsTracker.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace _750HrsTracker.Persistence.Seeds
{
    public class UpdateSubscriptionsPriceId
    {
        public static async Task SeedSubscriptionPricesAsync(AppDbContext context, AppSettings appSettings)
        {
            // get product           
            
            var subscriptions = await context.Subscriptions.ToListAsync();
            foreach (var subscription in subscriptions)
            {
                if (string.IsNullOrEmpty(subscription.StripePriceId))
                {
                    var options = new PriceCreateOptions()
                    {
                        UnitAmount = Convert.ToInt64(subscription.Price * 100),
                        LookupKey = subscription.Slug,
                        Currency = "usd",
                        Product = appSettings.StripeProductId
                    };
                    switch (subscription.SubscriptionInterval)
                    {
                        case SubscriptionInterval.monthly:
                            options.Recurring = new PriceRecurringOptions { Interval = "month" };
                            break;
                        case SubscriptionInterval.yearly:
                            options.Recurring = new PriceRecurringOptions { Interval = "year" };
                            break;
                        default:
                            continue;
                    }

                    var priceService = new PriceService();
                    Price price = await priceService.CreateAsync(options);

                    subscription.StripePriceId = price.Id;
                    
                }

               
            }

            context.Subscriptions.UpdateRange(subscriptions);
            await context.SaveChangesAsync();
        }
    }
}
