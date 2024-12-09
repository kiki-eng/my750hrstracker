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
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
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



        [AllowAnonymous]
        [Route("signup")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> SignUpAsync(SignUpRequest request)
        {
            ResponseHandler<string> response = new();
            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.Message = "error";
                return BadRequest(response);
            }
            response = await _userService.SignUpAsync(request, Request);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);

        }


        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
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

        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
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
        /// Update user details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
        [Route("get-permissions")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetUserPermissionsResponse>))]
        public async Task<IActionResult> GetUserPermissionsAsync()
        {
            ResponseHandler<GetUserPermissionsResponse> response = new ResponseHandler<GetUserPermissionsResponse>();

            response = await _userService.GetUserPermissionsAsync(Request);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
        [Route("{userId}")]
        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<GetUsersOnlyResponse>))]
        public async Task<IActionResult> UpdatetUserProfileAsync(Guid userId, UpdateUserRequest request)
        {
            ResponseHandler<GetUsersOnlyResponse> response = new ResponseHandler<GetUsersOnlyResponse>();

            response = await _userService.UpdatetUserProfileAsync(userId, request);

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
        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
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

        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
        [Route("{userId}/change-password")]
        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> ChangePasswordAsync(Guid userId, ChangePasswordRequest request)
        {
            ResponseHandler<string> response = await _userService.ChangePasswordAsync(userId, request);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [Route("verify-email")]
        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> VerifyEmailAsync(VerifyEmailRequest request)
        {
            ResponseHandler<string> response = await _userService.VerifyEmailAsync(request);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [Route("resend-verification-email")]
        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> ResendVerifyEmailAsync(ResendVerifyEmailRequest request)
        {
            ResponseHandler<string> response = await _userService.ResendVerifyEmailAsync(request, Request);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Update user profile pic
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
        [Route("{userId}/update-profile-pic")]
        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<UpdateProfilePictureRequest>))]
        public async Task<IActionResult> UpdatetUserProfilePictureAsync(Guid userId, UpdateProfilePictureRequest request)
        {
            ResponseHandler<UpdateProfilePictureRequest> response = await _userService.UpdatetUserProfilePictureAsync(userId, request);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Get user profile pic
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
        [Route("{userId}/switch-team")]
        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<string>))]
        public async Task<IActionResult> SwitchTeamAsync(Guid userId, SwitchTeamRequest request)
            => Ok(await _userService.SwitchTeamAsync(userId, request)); 
        
        /// <summary>
        /// Get user profile pic
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
        [Route("{userId}/profile-pic")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<Base64FileModel>))]
        public async Task<IActionResult> UpdatetUserProfilePictureAsync(Guid userId)
            => Ok(await _userService.GetUserProfilePictureAsync(userId));

        
        [Authorize(Policy = "AppUserPolicy", AuthenticationSchemes = "AppUserScheme")]
        [Route("{userId}/profile-pic")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseHandler<Base64FileModel>))]
        public async Task<IActionResult> RemoveProfilePictureAsync(Guid userId)
            => Ok(await _userService.RemoveProfilePictureAsync(userId));


    }
}
