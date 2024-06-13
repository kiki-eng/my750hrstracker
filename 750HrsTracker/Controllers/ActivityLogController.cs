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

        [Authorize(Policy = "Permission.ActivityLog.Create")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetActivityLogResponse>))]
        public async Task<IActionResult> AddActivityLogAsync(AddActivityLogRequest request)
           => Ok(await _activityLogService.AddActivityLogAsync(request));

        [Authorize(Policy = "Permission.ActivityLog.Create")]
        [HttpPost("add-str-log")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetActivityLogResponse>))]
        public async Task<IActionResult> AddSTRActivityLogAsync(BaseAddActivityLogRequest request)
           => Ok(await _activityLogService.AddSTRActivityLogAsync(request));

        [Authorize(Policy = "Permission.ActivityLog.View")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResponseHandler<List<GetActivityLogResponse>>))]
        public async Task<IActionResult> GetAllActivityLogAsync([FromQuery] PaginationFilter filter, [FromQuery] ActivityLogFilter activityLogFilter,
            [FromQuery] AvailablePropertyType propertyType = AvailablePropertyType.ALL)
            => Ok(await _activityLogService.GetAllActivityLogAsync(propertyType, filter, activityLogFilter, Request.Path));


        [Authorize(Policy = "Permission.ActivityLog.View")]
        [HttpGet("search")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<List<GetActivityLogResponse>>))]
        public async Task<IActionResult> SearchActivityLogAsync([FromQuery] string keyword)
            => Ok(await _activityLogService.SearchActivityLogAsync(keyword));

        [Authorize(Policy = "Permission.ActivityLog.View")]
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetActivityLogResponse>))]
        public async Task<IActionResult> GetActivityLogAsync(Guid id)
            => Ok(await _activityLogService.GetActivityLogAsync(id));

        [Authorize(Policy = "Permission.ActivityLog.Update")]
        [HttpPatch("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetActivityLogResponse>))]
        public async Task<IActionResult> UpdateActivityLogAsync(Guid id, AddActivityLogRequest request)
            => Ok(await _activityLogService.UpdateActivityLogAsync(id, request));


        [Authorize(Policy = "Permission.ActivityLog.Delete")]
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> DeleteActivityLogAsync(Guid id)
            => Ok(await _activityLogService.DeleteActivityLogAsync(id));

        [Authorize(Policy = "Permission.ActivityLog.Create")]
        [HttpGet("download-import-template")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<Base64FileModel>))]
        public async Task<IActionResult> DownloadActivityLogImportTemplateAsync()
            => Ok(await _activityLogService.DownloadActivityLogImportTemplateAsync());

        [Authorize(Policy = "Permission.ActivityLog.Import")]
        [HttpPost("import")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> ImportActivityLogAsync([FromQuery] AvailablePropertyType propertyType, ImportActivityLogRequest request)
            => Ok(await _activityLogService.ImportActivityLogAsync(propertyType, request));


        [Authorize(Policy = "Permission.ActivityLog.ExportDocument")]
        [HttpGet("download-documents")]
        public async Task<IActionResult> ExportDocumentsAsync([FromQuery] ExportDocumentFilter filter)
        {

            var zipBytes = await _activityLogService.ExportDocumentsAsync(filter);

            return File(zipBytes, "application/zip", "Log_Documents.zip");
        }





    }
}
