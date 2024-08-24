using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Extensions;
using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Services.Implementations;
using _750HrsTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ITeamService _tenantService;
        public SubscriptionsController(ISubscriptionService subscriptionService, ITeamService tenantService)
        {
            _subscriptionService = subscriptionService;
            _tenantService = tenantService;

        }

        [Authorize(AuthenticationSchemes = CustomPubAccessAuthenticationSchemeOption.Name)]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetSubscriptionResponse>))]
        public async Task<IActionResult> AddSubscriptionAsync(AddUpdateSubscriptionRequest request)
           => Ok(await _subscriptionService.AddSubscriptionAsync(request));

        [Authorize(AuthenticationSchemes = CustomPubAccessAuthenticationSchemeOption.Name)]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResponseHandler<List<GetSubscriptionResponse>>))]
        public async Task<IActionResult> GetAllSubscriptionsAsync([FromQuery] PaginationFilter filter)
            => Ok(await _subscriptionService.GetAllSubscriptionAsync(filter, Request.Path));
        
        [Authorize(AuthenticationSchemes = CustomPubAccessAuthenticationSchemeOption.Name)]
        [HttpGet("all")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<List<GetSubscriptionResponse>>))]
        public async Task<IActionResult> GetAllSubscriptionAsync()
            => Ok(await _subscriptionService.GetAllSubscriptionAsync());

        [Authorize(AuthenticationSchemes = CustomPubAccessAuthenticationSchemeOption.Name)]
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetSubscriptionResponse>))]
        public async Task<IActionResult> GetSubscriptionAsync(Guid id)
            => Ok(await _subscriptionService.GetSubscriptionAsync(id));

        [Authorize(AuthenticationSchemes = CustomPubAccessAuthenticationSchemeOption.Name)]
        [HttpPatch("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetSubscriptionResponse>))]
        public async Task<IActionResult> UpdateSubscriptionAsync(Guid id, AddUpdateSubscriptionRequest request)
            => Ok(await _subscriptionService.UpdateSubscriptionAsync(id, request));

        [Authorize(AuthenticationSchemes = CustomPubAccessAuthenticationSchemeOption.Name)]
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> DeleteSubscriptionAsync(Guid id)
          => Ok(await _subscriptionService.DeleteSubscriptionAsync(id));
        
        [Authorize(AuthenticationSchemes = CustomPubAccessAuthenticationSchemeOption.Name)]
        [HttpPut("{id}/update-permissions")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<UpdateSubscriptionPermissionResponse>))]
        public async Task<IActionResult> UpdateSubscriptionPermissionsAsync(Guid id, UpdateSubscriptionPermissionRequest request)
          => Ok(await _subscriptionService.UpdateSubscriptionPermissionsAsync(id, request));
        
        [Authorize(AuthenticationSchemes = CustomPubAccessAuthenticationSchemeOption.Name)]
        [HttpPut("{id}/update-features")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<UpdateSubscriptionPermissionResponse>))]
        public async Task<IActionResult> UpdateSubscriptionFeaturesAsync(Guid id, UpdateSubscriptionFeaturesRequest request)
          => Ok(await _subscriptionService.UpdateSubscriptionFeaturesAsync(id, request.Features!));

        [Authorize(AuthenticationSchemes = CustomPubAccessAuthenticationSchemeOption.Name)]
        [HttpPatch("{id}/update-stripe-price-id")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetSubscriptionResponse>))]
        public async Task<IActionResult> UpdateSubscriptionPriceAsync(Guid id, UpdateSubscriptionPriceRequest request)
          => Ok(await _subscriptionService.UpdateSubscriptionPriceAsync(id, request));

        [Authorize(Policy = "Permission.Subscription.Manage")]
        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
        [ServiceFilter(typeof(SessionFilter))]
        [HttpPost("stripe/create-checkout-session")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<CreateStripeCheckoutSessionResponse>))]
        public async Task<IActionResult> CreateStripeCheckoutSessionAsync(CreateStripeCheckoutSessionRequest request)
          => Ok(await _tenantService.CreateStripeCheckoutSessionAsync(request));


    }
}
