using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Models.SubscriptionModels;
using System.Text.Json;

namespace _750HrsTracker.Services.Interfaces
{
    public interface IWebhookNotificationService
    {
        Task<PagedResponseHandler<List<WebhookNotificationTraceLog>>> GetAllNotificationLogsAsync(PaginationFilter filter, string route);
        Task<ResponseHandler<string>> ProcessStripeWebhookNotificationAsync(HttpContext httpContext);
        Task<ResponseHandler<string>> ProcessAppStoreWebhookNotificationAsync(HttpContext httpContext);
    }
}
