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
    [ServiceFilter(typeof(SessionFilter))]
    public class ActivityLogController : ControllerBase
    {
        private readonly IActivityLogService _activityLogService;
        public ActivityLogController(IActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }


        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetActivityLogResponse>))]
        public async Task<IActionResult> AddActivityLogAsync(AddActivityLogRequest request)
           => Ok(await _activityLogService.AddActivityLogAsync(request));


        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResponseHandler<List<GetActivityLogResponse>>))]
        public async Task<IActionResult> GetAllActivityLogAsync([FromQuery] PaginationFilter filter, [FromQuery] ActivityLogFilter activityLogFilter)
            => Ok(await _activityLogService.GetAllActivityLogAsync(filter, activityLogFilter, Request.Path));


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

        [HttpGet("download-import-template")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<Base64FileModel>))]
        public async Task<IActionResult> DownloadActivityLogImportTemplateAsync()
            => Ok(await _activityLogService.DownloadActivityLogImportTemplateAsync());       
        
        [HttpPost("import")]
        public async Task<IActionResult> ImportActivityLogAsync()
        {
            // Path to the template CSV file
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "750hrsTracker_ActivityLogImportTemplate.csv");

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            // Return the file as a FileStreamResult
            //var fileStream = System.IO.File.OpenRead(filePath);

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", "750hrsTracker_ActivityLogImportTemplate.csv");
        }



    }
}
