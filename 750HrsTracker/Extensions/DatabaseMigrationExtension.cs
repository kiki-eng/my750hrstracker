using _750HrsTracker.Helpers;
using _750HrsTracker.Models;
using _750HrsTracker.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace _750HrsTracker.Extensions
{
    public static class DatabaseMigrationExtension
    {
        public static async void CustomRunMigration(this WebApplication app)
        {
            using(var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;   
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("App");

                var context = services.GetRequiredService<AppDbContext>();
                var roleManager = services.GetRequiredService<RoleManager<Role>>();

                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
                var appSettings = config.GetSection("ApplicationConfiguration").Get<AppSettings>();

                try
                {
                    bool runMigration = bool.Parse(app.Configuration.GetSection("ApplicationConfiguration:RunMigration").Value);

                    if (runMigration)
                    {
                        context.Database.Migrate();
                        logger.LogInformation("Database successfully updated");
                    }

                    await Persistence.Seeds.DefaultRoles.SeedAsync(context);
                    await Persistence.Seeds.DefaultPermission.SeedPermissionForRoleAsync(roleManager, context);
                    await Persistence.Seeds.DefaultLogData.SeedDefaultCatgoriesAsync(context);
                    await Persistence.Seeds.DefaultLogData.SeedDefaultLogActivityAsync(context);
                    await Persistence.Seeds.DefaultLogData.SeedDefaultStrLogActivityAsync(context);
                    await Persistence.Seeds.UpdateSubscriptionsPriceId.SeedSubscriptionPricesAsync(context, appSettings);
                    await Persistence.Seeds.DefaultAdmin.SeedAsync(context, appSettings);

                    logger.LogInformation("Application starting ...");


                }catch (Exception ex)
                {
                    logger.LogError(ex, "An error occured updating DB");
                }
            }
        }
    }
}
