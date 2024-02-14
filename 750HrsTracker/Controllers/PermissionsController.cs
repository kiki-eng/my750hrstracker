using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Services.Implementations;
using _750HrsTracker.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace _750HrsTracker.Controllers
{
    [Route("api/permissions")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        public PermissionsController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetPermissionResponse>))]
        public async Task<IActionResult> AddPermissionAsync(AddUpdatePermissionRequest request)
        => Ok(await _permissionService.AddPermissionAsync(request));


        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResponseHandler<List<GetPermissionResponse>>))]
        public async Task<IActionResult> GetAllPermissionsAsync([FromQuery] PaginationFilter filter)
            => Ok(await _permissionService.GetAllPermissionAsync(filter, Request.Path));

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetPermissionResponse>))]
        public async Task<IActionResult> GetPermissionAsync(Guid id)
            => Ok(await _permissionService.GetPermissionAsync(id));

        [HttpPatch("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetPermissionResponse>))]
        public async Task<IActionResult> UpdatePermissionAsync(Guid id, AddUpdatePermissionRequest request)
            => Ok(await _permissionService.UpdatePermissionAsync(id, request));

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> DeletePermissionAsync(Guid id)
          => Ok(await _permissionService.DeletePermissionAsync(id));
    }
}
