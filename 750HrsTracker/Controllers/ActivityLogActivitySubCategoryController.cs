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
    [Route("api/log-activities-sub-categories")]
    [ApiController]
    public class ActivityLogSubCategoryController : ControllerBase
    {
        private readonly IActivityLogSubCategoryService _logSubCategoryService;
        public ActivityLogSubCategoryController(IActivityLogSubCategoryService logSubCategoryService)
        {
            _logSubCategoryService = logSubCategoryService;
        }

        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
        [HttpPost("{logActivityId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetLogActivitySubCategoryResponse>))]
        public async Task<IActionResult> AddLogActivitySubCategoryAsync(Guid logActivityId, AddUpdateLogActivitySubCategoryRequest request)
           => Ok(await _logSubCategoryService.AddLogActivitySubCategoryAsync(logActivityId, request));


        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResponseHandler<List<GetLogActivitySubCategoryResponse>>))]
        public async Task<IActionResult> GetAllLogActivitySubCategoryAsync([FromQuery] PaginationFilter filter)
            => Ok(await _logSubCategoryService.GetAllLogActivitySubCategoryAsync(filter, Request.Path));


        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
        [HttpGet("search")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<List<GetLogActivitySubCategoryResponse>>))]
        public async Task<IActionResult> SearchLogActivitySubCategoryAsync([FromQuery] string keyword)
            => Ok(await _logSubCategoryService.SearchLogActivitySubCategoryAsync(keyword));

        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetLogActivitySubCategoryResponse>))]
        public async Task<IActionResult> GetLogActivitySubCategoryAsync(Guid id)
            => Ok(await _logSubCategoryService.GetLogActivitySubCategoryAsync(id));

        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
        [HttpPatch("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetLogActivitySubCategoryResponse>))]
        public async Task<IActionResult> UpdateLogActivitySubCategoryAsync(Guid id, AddUpdateLogActivitySubCategoryRequest request)
            => Ok(await _logSubCategoryService.UpdateLogActivitySubCategoryAsync(id, request));


        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> DeleteLogActivitySubCategoryAsync(Guid id)
            => Ok(await _logSubCategoryService.DeleteLogActivitySubCategoryAsync(id));
    }
}
