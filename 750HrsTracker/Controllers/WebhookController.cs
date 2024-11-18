using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.Enums;
using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Models.SubscriptionModels;
using _750HrsTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace _750HrsTracker.Controllers
{
    [AllowAnonymous]
    [Route("api/webhook")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly IWebhookNotificationService _webhookNotificationService;
        public WebhookController(IWebhookNotificationService webhookNotificationService)
        {
            _webhookNotificationService = webhookNotificationService;
        }

        [HttpGet("notification-logs")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResponseHandler<List<WebhookNotificationTraceLog>>))]
        public async Task<IActionResult> GetAllNotificationLogsAsync([FromQuery] PaginationFilter filter)
            => Ok(await _webhookNotificationService.GetAllNotificationLogsAsync(filter, Request.Path));


        [HttpPost("stripe-notifications")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> ProcessStripWebhookNotificationAsync()
        {
            var response = await _webhookNotificationService.ProcessStripeWebhookNotificationAsync(HttpContext);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        
        //[HttpPost("in-app-purchase-notifications")]
        //[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        //public async Task<IActionResult> ProcessStripWebhookNotificationAsync()
        //{
        //    var response = await _webhookNotificationService.ProcessStripeWebhookNotificationAsync(HttpContext);

        //    if (!response.Success)
        //    {
        //        return BadRequest(response);
        //    }

        //    return Ok(response);
        //}
    }
}
