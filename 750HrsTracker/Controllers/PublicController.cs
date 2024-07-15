using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Extensions;
using _750HrsTracker.Helpers;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Services.Implementations;
using _750HrsTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace _750HrsTracker.Controllers
{
    [Route("api/public")]
    [Authorize(AuthenticationSchemes = CustomPubAccessAuthenticationSchemeOption.Name)]
    [ApiController]
    public class PublicController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public PublicController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("send-support-notification")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> SendSupportNotificationAsync(SupportRequest request)
            => Ok(await _adminService.SendSupportNotificationAsync(request));
        
        [HttpGet("subsriptions")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<List<GetSubscriptionResponse>>))]
        public async Task<IActionResult> GetSubscriptionsAsync()
            => Ok(await _adminService.GetSubscriptionsAsync());
        
        
        [HttpGet("autocomplete/{keyword}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetAutoSuggestionResponse>))]
        public async Task<IActionResult> GetAutocompleteSuggestions(string keyword)
            => Ok(await _adminService.GetAutoSuggestionResponseAsync(keyword));

    }
}
