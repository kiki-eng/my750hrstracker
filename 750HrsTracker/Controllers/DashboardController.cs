using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Enums;
using _750HrsTracker.Filters;
using _750HrsTracker.Models;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Services.Implementations;
using _750HrsTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace _750HrsTracker.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(SessionFilter))]
    public class DashboardController : ControllerBase
    {
        private readonly IActivityLogService _activityLogService;
        public DashboardController(IActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }
        /// <summary>
        /// Get Recent activity logs
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("get-data")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetDashboardResponse>))]
        public async Task<IActionResult> GetDashboardDataAsync([FromQuery] AvailablePropertyType propertyType)
            => Ok(await _activityLogService.GetDashboardDataAsync(propertyType));
    }
}
