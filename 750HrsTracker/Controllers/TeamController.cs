using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
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
    [Route("api/team")]
    [ApiController]
    [ServiceFilter(typeof(SessionFilter))]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;
        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [Authorize(Policy = "Permission.Team.AddRole")]
        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
        [HttpPost("roles")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetRoleResponse>))]
        public async Task<IActionResult> AddTeamRoleAsync(AddRoleRequest request)
            => Ok(await _teamService.AddTeamRoleAsync(request));
        
        [Authorize(Policy = "Permission.Team.GetRole")]
        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
        [HttpGet("roles")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResponseHandler<List<GetRoleResponse>>))]
        public async Task<IActionResult> GetTeamRolesAsync([FromQuery] PaginationFilter filter)
            => Ok(await _teamService.GetTeamRolesAsync(filter, Request.Path));

        [Authorize(Policy = "Permission.Team.UpdateRole")]
        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
        [HttpPatch("roles/{roleId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetRoleResponse>))]
        public async Task<IActionResult> UpdatetRoleAsync(Guid roleId, UpdateRoleRequest request)
            => Ok(await _teamService.UpdatetRoleAsync(roleId, request));
        
        [Authorize(Policy = "Permission.Team.DeleteRole")]
        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
        [HttpDelete("roles/{roleId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetRoleResponse>))]
        public async Task<IActionResult> DeleteRoleAsync(Guid roleId)
            => Ok(await _teamService.DeleteRoleAsync(roleId));

        [Authorize(Policy = "Permission.Team.UpdateRole")]
        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
        [HttpPatch("roles/{roleId}/update-permissions")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetRoleResponse>))]
        public async Task<IActionResult> UpdateRolePermissionsAsync(Guid roleId, UpdateRolePermissionsRequest request)
            => Ok(await _teamService.UpdateRolePermissionsAsync(roleId, request));

        [Authorize(Policy = "Permission.Team.InviteUser")]
        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
        [HttpPost("invite-user")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<InviteUserResponse>))]
        public async Task<IActionResult> InviteUserAsync(InviteUserRequest request)
            => Ok(await _teamService.InviteUserAsync(request, Request));

        [AllowAnonymous]
        [HttpPost("validate-invitation")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> ValidateInvitationAsync(ValidateInvitationRequest request)
            => Ok(await _teamService.ValidateInvitationAsync(request));

        [AllowAnonymous]
        [HttpPost("create-invited-user")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> CreateInvitedUserAsync(CreateInvitedUserRequest request)
            => Ok(await _teamService.CreateInvitedUserAsync(request));
        
        [Authorize(Policy = "Permission.Team.InviteUser")]
        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
        [HttpGet("pending-invitation")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<List<PendingUserInvitationResponse>>))]
        public async Task<IActionResult> GetPendingUserInvitationsAsync()
            => Ok(await _teamService.GetPendingUserInvitationsAsync());
        
        [Authorize(Policy = "Permission.Team.GetUsers")]
        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
        [HttpGet("get-users")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResponseHandler<List<GetUsersOnlyResponse>>))]
        public async Task<IActionResult> GetTeamUsersAsync([FromQuery] PaginationFilter filter)
            => Ok(await _teamService.GetTeamUsersAsync(filter, Request.Path));

        [AllowAnonymous]
        [HttpGet("get-users-once")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<List<GetUsersOnlyResponse>>))]
        public async Task<IActionResult> GetTeamUsersAsync()
            => Ok(await _teamService.GetTeamUsersAsync());


        [Authorize(Policy = "Permission.Team.MakeSpouse")]
        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
        [HttpPost("make-spouse")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetUsersOnlyResponse>))]
        public async Task<IActionResult> MaKeSpouseRequestAsync(MakeSpouseRequest request)
            => Ok(await _teamService.MaKeSpouseRequestAsync(request));
        
        [Authorize(Policy = "Permission.Team.ActivateDeactivate")]
        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
        [HttpPatch("activate-deactivate-user")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetUsersOnlyResponse>))]
        public async Task<IActionResult> ActivateDeactivateUsersAsync(MakeSpouseRequest request)
            => Ok(await _teamService.ActivateDeactivateUsersAsync(request));
        
        [Authorize(Policy = "Permission.Team.UpdateRole")]
        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
        [HttpPatch("update-user-role")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> UpdateUserRoleAsync(UpdateUserRoleRequest request)
            => Ok(await _teamService.UpdateUserRoleAsync(request));
        
        [Authorize(Policy = "Permission.Team.DeleteAccount")]
        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
        [HttpDelete("delete-account")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetUsersOnlyResponse>))]
        public async Task<IActionResult> DeactivateAccountAsync()
            => Ok(await _teamService.DeactivateAccountAsync());

    }
}
