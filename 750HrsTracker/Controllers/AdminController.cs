using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Enums;
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
    [Route("api/admin")]
    [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
    [AllowAnonymous]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("teams")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResponseHandler<List<GetTeamResponse>>))]
        public async Task<IActionResult> GetAllTeamsAsync([FromQuery] PaginationFilter filter)
            => Ok(await _adminService.GetAllTeamsAsync(filter, Request));
        
        
        [HttpGet("teams/{teamId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<AdminGetTeamResponse>))]
        public async Task<IActionResult> GetTeamAsync(Guid teamId)
            => Ok(await _adminService.GetTeamAsync(teamId));

        #region Activity Logs
        [HttpGet("activity-logs")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResponseHandler<List<GetActivityLogResponse>>))]
        public async Task<IActionResult> GetAllActivityLogAsync([FromQuery] PaginationFilter filter, [FromQuery] ActivityLogFilter activityLogFilter,
            [FromQuery] AvailablePropertyType propertyType = AvailablePropertyType.ALL)
            => Ok(await _adminService.GetAllActivityLogAsync(propertyType, filter, activityLogFilter, Request.Path));


        [HttpGet("activity-logs/search")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<List<GetActivityLogResponse>>))]
        public async Task<IActionResult> SearchActivityLogAsync([FromQuery] string keyword)
            => Ok(await _adminService.SearchActivityLogAsync(keyword));

        [HttpGet("activity-logs/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetActivityLogResponse>))]
        public async Task<IActionResult> GetActivityLogAsync(Guid id)
            => Ok(await _adminService.GetActivityLogAsync(id));

        #endregion

        #region Properties
        [HttpGet("properties")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResponseHandler<List<AdminGetPropertyResponse>>))]
        public async Task<IActionResult> GetAllPropertiesAsync([FromQuery] PaginationFilter filter)
            => Ok(await _adminService.GetAllPropertiesAsync(filter, Request.Path));


        [HttpGet("properties/search")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<List<AdminGetPropertyResponse>>))]
        public async Task<IActionResult> SearchPropertiesAsync([FromQuery] string keyword)
            => Ok(await _adminService.SearchPropertiesAsync(keyword));

        [HttpGet("properties/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<AdminGetPropertyResponse>))]
        public async Task<IActionResult> GetPropertyAsync(Guid id)
            => Ok(await _adminService.GetPropertyAsync(id));

        #endregion
    }
}
