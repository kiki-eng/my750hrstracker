using _750HrsTracker.Repositories.Implementations;
using _750HrsTracker.Repositories.Interfaces;
using _750HrsTracker.Services.Implementations;
using _750HrsTracker.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace _750HrsTracker.Extensions
{
    public static class ServicesDIExtension
    {
        public static IServiceCollection AddServicesFromExtension(this IServiceCollection service)
        {
            //repository  
            service.AddScoped<IUserRepository, UserRepository>();

            // services
            service.TryAddTransient<IUriService, UriService>();
            service.TryAddScoped<IUserService, UserService>();
            service.TryAddScoped<INotificationService, NotificationService>();
            service.TryAddScoped<IEmailService, EmailService>();


            return service;
        }
    }
}
 