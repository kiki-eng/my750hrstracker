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
    [Route("api/admin-users")]
    [ApiController]
    public class AdminUserController : ControllerBase
    {
        private readonly IAdminUserService _userService;
        public AdminUserController(IAdminUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [Route("signin")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<SignInResponse>))]
        public async Task<IActionResult> SignInAsync(SignInRequest request)
        {
            ResponseHandler<SignInResponse> response = new ResponseHandler<SignInResponse>();
            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.Message = "error";
                return BadRequest(response);
            }
            response = await _userService.SignInAsync(request, Request);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);

        }

        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetUsersOnlyResponse>))]
        public async Task<IActionResult> GetUserAsync(Guid id)
        {
            ResponseHandler<GetUsersOnlyResponse> response = new ResponseHandler<GetUsersOnlyResponse>();
           
            response = await _userService.GetUserAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
        [Route("me")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetUsersOnlyResponse>))]
        public async Task<IActionResult> GetUserByTokenAsync()
        {
            ResponseHandler<GetUsersOnlyResponse> response = new ResponseHandler<GetUsersOnlyResponse>();

            response = await _userService.GetUserByTokenAsync(Request);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }





        /// <summary>
        /// Update user security setup e.g Notification on Login
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
        [Route("{userId}/update-security")]
        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<UpdateUserSecurityRequest>))]
        public async Task<IActionResult> UpdateMerchantUserSecurity(Guid userId, UpdateUserSecurityRequest request)
        {
            ResponseHandler<UpdateUserSecurityRequest> response = new ResponseHandler<UpdateUserSecurityRequest>();
           
            response = await _userService.UpdateUserSecurityAsync(userId, request);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }       

        [AllowAnonymous]
        [Route("recover-password")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> RecoverPasswordAsync(RecoverPasswordRequest request)
        {
            ResponseHandler<string> response = await _userService.RecoverPasswordAsync(request, Request);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [Route("reset-password")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ResponseHandler<string> response = await _userService.ResetPasswordAsync(request);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = "AdminScheme")]
        [Route("{userId}/change-password")]
        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> ChangePasswordAsync(Guid merchantUserId, ChangePasswordRequest request)
        {
            ResponseHandler<string> response = await _userService.ChangePasswordAsync(merchantUserId, request);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        
    }
}
