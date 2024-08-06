using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace _750HrsTracker.Controllers
{
    [Route("api/log-categories")]
    [ApiController]
    public class ActivityLogCategoryController : ControllerBase
    {
        private readonly IActivityLogCategoryService _logCategoryService;
        public ActivityLogCategoryController(IActivityLogCategoryService logCategoryService)
        {
            _logCategoryService = logCategoryService;
        }

        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetActivityLogCategoryResponse>))]
        public async Task<IActionResult> AddActivityLogCategoryAsync(AddActivityLogCategoryRequest request)
           => Ok(await _logCategoryService.AddActivityLogCategoryAsync(request));


        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResponseHandler<List<GetActivityLogCategoryResponse>>))]
        public async Task<IActionResult> GetAllActivityLogCategoryAsync([FromQuery] PaginationFilter filter)
            => Ok(await _logCategoryService.GetAllActivityLogCategoryAsync(filter, Request.Path));

        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
        [HttpGet("list")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<List<GetTimeAndLogCategoryResponse>>))]
        public async Task<IActionResult> GetAllActivityLogCategoryAsync()
            => Ok(await _logCategoryService.GetAllActivityLogCategoryAsync());


        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
        [HttpGet("search")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<List<GetActivityLogCategoryResponse>>))]
        public async Task<IActionResult> SearchActivityLogCategoryAsync([FromQuery] string keyword)
            => Ok(await _logCategoryService.SearchActivityLogCategoryAsync(keyword));

        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetActivityLogCategoryResponse>))]
        public async Task<IActionResult> GetActivityLogCategoryAsync(Guid id)
            => Ok(await _logCategoryService.GetActivityLogCategoryAsync(id));

        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
        [HttpPatch("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetActivityLogCategoryResponse>))]
        public async Task<IActionResult> UpdateActivityLogCategoryAsync(Guid id, UpdateActivityLogCategoryRequest request)
            => Ok(await _logCategoryService.UpdateActivityLogCategoryAsync(id, request));


        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> DeleteActivityLogCategoryAsync(Guid id)
            => Ok(await _logCategoryService.DeleteActivityLogCategoryAsync(id));

    }
}
