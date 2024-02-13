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
    [Route("api/log-activities-sub-categories")]
    [ApiController]
    public class ActivityLogSubCategoryController : ControllerBase
    {
        private readonly IActivityLogSubCategoryService _logSubCategoryService;
        public ActivityLogSubCategoryController(IActivityLogSubCategoryService logSubCategoryService)
        {
            _logSubCategoryService = logSubCategoryService;
        }

        [HttpPost("{logActivityId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetLogActivitySubCategoryResponse>))]
        public async Task<IActionResult> AddLogActivitySubCategoryAsync(Guid logActivityId, AddUpdateLogActivitySubCategoryRequest request)
           => Ok(await _logSubCategoryService.AddLogActivitySubCategoryAsync(logActivityId, request));


        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResponseHandler<List<GetLogActivitySubCategoryResponse>>))]
        public async Task<IActionResult> GetAllLogActivitySubCategoryAsync([FromQuery] PaginationFilter filter)
            => Ok(await _logSubCategoryService.GetAllLogActivitySubCategoryAsync(filter, Request.Path));


        [HttpGet("search")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<List<GetLogActivitySubCategoryResponse>>))]
        public async Task<IActionResult> SearchLogActivitySubCategoryAsync([FromQuery] string keyword)
            => Ok(await _logSubCategoryService.SearchLogActivitySubCategoryAsync(keyword));

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetLogActivitySubCategoryResponse>))]
        public async Task<IActionResult> GetLogActivitySubCategoryAsync(Guid id)
            => Ok(await _logSubCategoryService.GetLogActivitySubCategoryAsync(id));

        [HttpPatch("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetLogActivitySubCategoryResponse>))]
        public async Task<IActionResult> UpdateLogActivitySubCategoryAsync(Guid id, AddUpdateLogActivitySubCategoryRequest request)
            => Ok(await _logSubCategoryService.UpdateLogActivitySubCategoryAsync(id, request));


        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> DeleteLogActivitySubCategoryAsync(Guid id)
            => Ok(await _logSubCategoryService.DeleteLogActivitySubCategoryAsync(id));
    }
}
