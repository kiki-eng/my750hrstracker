using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Services.Interfaces;
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

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetActivityLogCategoryResponse>))]
        public async Task<IActionResult> AddActivityLogCategoryAsync(AddActivityLogCategoryRequest request)
           => Ok(await _logCategoryService.AddActivityLogCategoryAsync(request));


        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResponseHandler<List<GetActivityLogCategoryResponse>>))]
        public async Task<IActionResult> GetAllActivityLogCategoryAsync([FromQuery] PaginationFilter filter)
            => Ok(await _logCategoryService.GetAllActivityLogCategoryAsync(filter, Request.Path));
        
        [HttpGet("list")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<List<GetLogCategoryResponse>>))]
        public async Task<IActionResult> GetAllActivityLogCategoryAsync()
            => Ok(await _logCategoryService.GetAllActivityLogCategoryAsync());


        [HttpGet("search")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<List<GetActivityLogCategoryResponse>>))]
        public async Task<IActionResult> SearchActivityLogCategoryAsync([FromQuery] string keyword)
            => Ok(await _logCategoryService.SearchActivityLogCategoryAsync(keyword));

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetActivityLogCategoryResponse>))]
        public async Task<IActionResult> GetActivityLogCategoryAsync(Guid id)
            => Ok(await _logCategoryService.GetActivityLogCategoryAsync(id));

        [HttpPatch("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetActivityLogCategoryResponse>))]
        public async Task<IActionResult> UpdateActivityLogCategoryAsync(Guid id, UpdateActivityLogCategoryRequest request)
            => Ok(await _logCategoryService.UpdateActivityLogCategoryAsync(id, request));


        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> DeleteActivityLogCategoryAsync(Guid id)
            => Ok(await _logCategoryService.DeleteActivityLogCategoryAsync(id));

    }
}
