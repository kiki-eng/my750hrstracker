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
    [Route("api/teams")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(SessionFilter))]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;
        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost("invite-user")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<InviteUserResponse>))]
        public async Task<IActionResult> InviteUserAsync(InviteUserRequest request)
            => Ok(await _teamService.InviteUserAsync(request, Request));
        
        [HttpPost("validate-invitation")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> ValidateInvitationAsync(ValidateInvitationRequest request)
            => Ok(await _teamService.ValidateInvitationAsync(request));
        
        [HttpPost("create-invited-user")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> CreateInvitedUserAsync(CreateInvitedUserRequest request)
            => Ok(await _teamService.CreateInvitedUserAsync(request));
        
        [HttpGet("pending-invitation")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<List<PendingUserInvitationResponse>>))]
        public async Task<IActionResult> GetPendingUserInvitationsAsync()
            => Ok(await _teamService.GetPendingUserInvitationsAsync());



    }
}
