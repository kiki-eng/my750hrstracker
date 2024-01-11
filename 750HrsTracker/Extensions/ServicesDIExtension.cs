using _750HrsTracker.Services.Implementations;
using _750HrsTracker.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace _750HrsTracker.Extensions
{
    public static class ServicesDIExtension
    {
        public static IServiceCollection AddServicesFromExtension(this IServiceCollection service)
        {
            service.TryAddTransient<IUriService, UriService>();


            return service;
        }
    }
}
