using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Services.Implementations;
using _750HrsTracker.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace _750HrsTracker.Controllers
{
    [Route("api/subscriptions")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        public SubscriptionsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetSubscriptionResponse>))]
        public async Task<IActionResult> AddSubscriptionAsync(AddUpdateSubscriptionRequest request)
           => Ok(await _subscriptionService.AddSubscriptionAsync(request));


        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResponseHandler<List<GetSubscriptionResponse>>))]
        public async Task<IActionResult> GetAllSubscriptionsAsync([FromQuery] PaginationFilter filter)
            => Ok(await _subscriptionService.GetAllSubscriptionAsync(filter, Request.Path));
        
        [HttpGet("all")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<List<GetSubscriptionResponse>>))]
        public async Task<IActionResult> GetAllSubscriptionAsync()
            => Ok(await _subscriptionService.GetAllSubscriptionAsync());

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetSubscriptionResponse>))]
        public async Task<IActionResult> GetSubscriptionAsync(Guid id)
            => Ok(await _subscriptionService.GetSubscriptionAsync(id));

        [HttpPatch("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetSubscriptionResponse>))]
        public async Task<IActionResult> UpdateSubscriptionAsync(Guid id, AddUpdateSubscriptionRequest request)
            => Ok(await _subscriptionService.UpdateSubscriptionAsync(id, request));

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> DeleteSubscriptionAsync(Guid id)
          => Ok(await _subscriptionService.DeleteSubscriptionAsync(id));
    }
}
