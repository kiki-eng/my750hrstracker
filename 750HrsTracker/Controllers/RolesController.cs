using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Extensions;
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
    [Route("api/roles")]
    [Authorize(AuthenticationSchemes = CustomAdminAuthenticationSchemeOption.Name)]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly  IRolesService _roleService;
        public RolesController(IRolesService roleService)
        {
            _roleService = roleService;
        }


        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResponseHandler<List<GetRoleResponse>>))]
        public async Task<IActionResult> GetAllPermissionsAsync([FromQuery] PaginationFilter filter)
            => Ok(await _roleService.GetAllRolesAsync(filter, Request.Path));


        [HttpPatch("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetRoleResponse>))]
        public async Task<IActionResult> UpdatePermissionAsync(Guid id, AddUpdateRolesRequest request)
            => Ok(await _roleService.UpdateRoleAsync(id, request));

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> DeletePermissionAsync(Guid id)
          => Ok(await _roleService.DeleteRoleAsync(id));
    }
}
