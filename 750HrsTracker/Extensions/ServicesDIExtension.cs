using _750HrsTracker.Filters;
using _750HrsTracker.Providers;
using _750HrsTracker.Repositories.Implementations;
using _750HrsTracker.Repositories.Interfaces;
using _750HrsTracker.Services.Implementations;
using _750HrsTracker.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace _750HrsTracker.Extensions
{
    public static class ServicesDIExtension
    {
        public static IServiceCollection AddServicesFromExtension(this IServiceCollection services)
        {
            //repository  
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();

            // services
            services.TryAddTransient<IUriService, UriService>();
            services.TryAddScoped<IUserService, UserService>();
            services.TryAddScoped<INotificationService, NotificationService>();
            services.TryAddScoped<IEmailService, EmailService>();
            services.TryAddScoped<IPropertyService, PropertyService>();


            services.AddSingleton<SessionProvider>();
            services.AddScoped<SessionFilter>();


            return services;
        }
    }
}
 