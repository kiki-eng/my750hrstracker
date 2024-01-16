using _750HrsTracker.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace _750HrsTracker.Extensions
{
    public static class DatabaseMigrationExtension
    {
        public static void CustomRunMigration(this WebApplication app)
        {
            using(var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;   
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("App");

                var context = services.GetRequiredService<AppDbContext>();

                try
                {
                    bool runMigration = bool.Parse(app.Configuration.GetSection("ApplicationConfiguration:RunMigration").Value);

                    if (runMigration)
                    {
                        context.Database.Migrate();
                        logger.LogInformation("Database successfully updated");
                    }

                    logger.LogInformation("Application starting ...");
                }catch (Exception ex)
                {
                    logger.LogError(ex, "An error occured updating DB");
                }
            }
        }
    }
}
