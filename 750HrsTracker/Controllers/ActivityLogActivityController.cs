using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Enums;
using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace _750HrsTracker.Controllers
{
    [Route("api/log-activities")]
    [ApiController]
    public class ActivityLogActivityController : ControllerBase
    {
        private readonly IActivityLogActivityService _logActivityService;
        public ActivityLogActivityController(IActivityLogActivityService logActivityService)
        {
            _logActivityService = logActivityService;
        }

        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetActivityLogActivityResponse>))]
        public async Task<IActionResult> AddActivityLogActivityAsync(AddUpdateActivityLogActivityRequest request)
           => Ok(await _logActivityService.AddActivityLogActivityAsync(request));


        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResponseHandler<List<GetActivityLogActivityResponse>>))]
        public async Task<IActionResult> GetAllActivityLogActivityAsync([FromQuery] PaginationFilter filter)
            => Ok(await _logActivityService.GetAllActivityLogActivityAsync(filter, Request.Path));


        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
        [HttpGet("list")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<List<GetActivityLogActivityResponse>>))]
        public async Task<IActionResult> GetAllActivityLogActivityAsync([FromQuery] AvailablePropertyType availablePropertyType)
            => Ok(await _logActivityService.GetAllActivityLogActivityAsync(availablePropertyType));
        
        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
        [HttpGet("search")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<List<GetActivityLogActivityResponse>>))]
        public async Task<IActionResult> SearchActivityLogActivityAsync([FromQuery] string keyword)
            => Ok(await _logActivityService.SearchActivityLogActivityAsync(keyword));

        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetActivityLogActivityResponse>))]
        public async Task<IActionResult> GetActivityLogActivityAsync(Guid id)
            => Ok(await _logActivityService.GetActivityLogActivityAsync(id));

        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
        [HttpPatch("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetActivityLogActivityResponse>))]
        public async Task<IActionResult> UpdateActivityLogActivityAsync(Guid id, AddUpdateActivityLogActivityRequest request)
            => Ok(await _logActivityService.UpdateActivityLogActivityAsync(id, request));


        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> DeleteActivityLogActivityAsync(Guid id)
            => Ok(await _logActivityService.DeleteActivityLogActivityAsync(id));

    }
}
