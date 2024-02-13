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
    [Route("api/activity-logs")]
    [Authorize]
    [ApiController]
    public class ActivityLogController : ControllerBase
    {
        private readonly IActivityLogService _activityLogService;
        public ActivityLogController(IActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }


        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetActivityLogResponse>))]
        public async Task<IActionResult> AddActivityLogAsync([FromForm] AddActivityLogRequest request)
           => Ok(await _activityLogService.AddActivityLogAsync(request));


        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResponseHandler<List<GetActivityLogResponse>>))]
        public async Task<IActionResult> GetAllActivityLogAsync([FromQuery] PaginationFilter filter)
            => Ok(await _activityLogService.GetAllActivityLogAsync(filter, Request.Path));


        [HttpGet("search")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<List<GetActivityLogResponse>>))]
        public async Task<IActionResult> SearchActivityLogAsync([FromQuery] string keyword)
            => Ok(await _activityLogService.SearchActivityLogAsync(keyword));

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetActivityLogResponse>))]
        public async Task<IActionResult> GetActivityLogAsync(Guid id)
            => Ok(await _activityLogService.GetActivityLogAsync(id));

        [HttpPatch("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetActivityLogResponse>))]
        public async Task<IActionResult> UpdateActivityLogAsync(Guid id, UpdateActivityLogRequest request)
            => Ok(await _activityLogService.UpdateActivityLogAsync(id, request));


        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> DeleteActivityLogAsync(Guid id)
            => Ok(await _activityLogService.DeleteActivityLogAsync(id));


    }
}
