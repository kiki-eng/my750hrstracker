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

    
    [Route("api/properties")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(SessionFilter))]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetPropertyResponse>))]
        public async Task<IActionResult> AddPropertyAsync(AddUpdatePropertyRequest request)
            => Ok(await _propertyService.AddPropertyAsync(request));
        
        
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResponseHandler<List<GetPropertyResponse>>))]
        public async Task<IActionResult> GetAllPropertiesAsync( [FromQuery] PaginationFilter filter, [FromQuery] AvailablePropertyType propertyType = AvailablePropertyType.ALL)
            => Ok(await _propertyService.GetAllPropertiesAsync(filter, Request.Path, propertyType));
        
        [HttpGet("list")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<List<GetPropertyResponse>>))]
        public async Task<IActionResult> GetAllPropertiesAsync([FromQuery] AvailablePropertyType propertyType = AvailablePropertyType.ALL)
            => Ok(await _propertyService.GetAllPropertiesAsync(propertyType));
        
        
        [HttpGet("search")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<List<GetPropertyResponse>>))]
        public async Task<IActionResult> SearchPropertiesAsync([FromQuery] string keyword)
            => Ok(await _propertyService.SearchPropertiesAsync(keyword));
        
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetPropertyResponse>))]
        public async Task<IActionResult> GetPropertyAsync(Guid id)
            => Ok(await _propertyService.GetPropertyAsync(id));
        
        [HttpPatch("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetPropertyResponse>))]
        public async Task<IActionResult> UpdatePropertyAsync(Guid id, AddUpdatePropertyRequest request)
            => Ok(await _propertyService.UpdatePropertyAsync(id, request));
        
        
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> DeletePropertyAsync(Guid id)
            => Ok(await _propertyService.DeletePropertyAsync(id));
        
      
        [HttpPost("{id}/assing-to-users")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> AssignPropertyToUsersAsync(Guid id, AssignPropertyToUsersRequest request)
            => Ok(await _propertyService.AssignPropertyToUsersAsync(id, request));
    }
}
