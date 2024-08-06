using _750HrsTracker.Filters;
using _750HrsTracker.PermissionManagement;
using _750HrsTracker.Providers;
using _750HrsTracker.Repositories.Implementations;
using _750HrsTracker.Repositories.Interfaces;
using _750HrsTracker.Services.Implementations;
using _750HrsTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
            services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
            services.AddScoped<IActivityLogActivityRepository, ActivityLogActivityRepository>();
            services.AddScoped<IActivityLogCategoryRepository, ActivityLogCategoryRepository>();
            services.AddScoped<IActivityLogSubCategoryRepository, ActivityLogSubCategoryRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<ITeamSubscriptionRepository, TeamSubscriptionRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IWebhookNotificationRepository, WebhooNotificationRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IAdminUserRepository, AdminUserRepository>();
            

            // services
            services.TryAddTransient<IUriService, UriService>();
            services.TryAddScoped<IUserService, UserService>();
            services.TryAddScoped<INotificationService, NotificationService>();
            services.TryAddScoped<IEmailService, EmailService>();
            services.TryAddScoped<IPropertyService, PropertyService>();
            services.TryAddScoped<IActivityLogService, ActivityLogService>();   
            services.TryAddScoped<IActivityLogActivityService, ActivityLogActivityService>();
            services.TryAddScoped<IActivityLogCategoryService, ActivityLogCategoryService>();
            services.TryAddScoped<IActivityLogSubCategoryService, ActivityLogSubCategoryService>();
            services.TryAddScoped<ISubscriptionService, SubscriptionService>();
            services.AddTransient<IAdminService, AdminService>();
            services.TryAddScoped<IPermissionService, PermissionService>();
            services.TryAddScoped<ITeamService,TeamService>();
            services.TryAddScoped<IDocumentService, DocumentService>();  
            services.TryAddScoped<IRolesService, RolesService>();  
            services.TryAddScoped<IAdminUserService, AdminUserService>();   

            services.TryAddScoped<IWebhookNotificationService, WebhookNotificationService>(); 


            services.AddSingleton<SessionProvider>();
            services.AddScoped<SessionFilter>();

            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();


            return services;
        }
    }
}
 