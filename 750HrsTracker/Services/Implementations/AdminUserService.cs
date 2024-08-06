using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Helpers;
using _750HrsTracker.Models;
using _750HrsTracker.Models.Admin;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Repositories.Implementations;
using _750HrsTracker.Repositories.Interfaces;
using _750HrsTracker.Services.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace _750HrsTracker.Services.Implementations
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IUriService _uriService;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly IAdminUserRepository _adminUserRepository;
        private readonly INotificationService _notificationService;
        public AdminUserService(IUriService uriService, IOptionsSnapshot<AppSettings> appSettings, 
            IMapper mapper, IAdminUserRepository adminUserRepository, INotificationService notificationService)
        {
            _appSettings = appSettings.Value;
            _uriService = uriService;
            _mapper = mapper;
            _adminUserRepository = adminUserRepository;
            _notificationService = notificationService;

        }

        public async Task<ResponseHandler<string>> ChangePasswordAsync(Guid userId, ChangePasswordRequest request)
        {
            try
            {
                ResponseHandler<string> response = new ResponseHandler<string>();

                var user = await _adminUserRepository.ChangePasswordAsync(userId, request.OldPassword!, request.NewPassword!);

                response.Success = true;
                response.Message = "Password changed successfully";
                response.Data = null;
                return response;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseHandler<GetUsersOnlyResponse>> GetUserAsync(Guid userId)
        {
            try
            {
                ResponseHandler<GetUsersOnlyResponse> response = new ResponseHandler<GetUsersOnlyResponse>();


                Admin user = await _adminUserRepository.GetSingleOrDefaultAsync(userId) ?? throw new KeyNotFoundException("user not found");

                response.Success = true;
                response.Message = "User retrieved successfully";
                response.Data = _mapper.Map<GetUsersOnlyResponse>(user);
                return response;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseHandler<GetUsersOnlyResponse>> GetUserByTokenAsync(HttpRequest httpRequest)
        {
            try
            {
                ResponseHandler<GetUsersOnlyResponse> response = new ResponseHandler<GetUsersOnlyResponse>();

                var details = Utility.GetUserIdFromToken(httpRequest);

                if(details.Item1 is null)
                {
                    throw new ApplicationException("Invalid user token");
                }


                Admin user = await _adminUserRepository.GetSingleOrDefaultAsync(Guid.Parse(details.Item1)) ?? throw new KeyNotFoundException("user not found");
                var responseData = MappedResponse(user);
                response.Success = true;
                response.Message = "Admin retrieved successfully";
                response.Data = responseData;
                return response;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseHandler<string>> RecoverPasswordAsync(RecoverPasswordRequest request, HttpRequest httpRequest)
        {
            ResponseHandler<string> response = new ResponseHandler<string>();
            try
            {

                var merchantUser = await _adminUserRepository.RecoverPasswordAsync(request.EmailAddress!);

                PasswordResetNotificationRequest notificationRequest = new PasswordResetNotificationRequest
                {
                    RecipientName = merchantUser.Firstname,
                    RecipientEmail = merchantUser.Email,
                    ResetPasswordToken = merchantUser.ResetToken,
                    Origin = _appSettings.NotificationOrigin,
                    OriginIpAddress = Utility.GetRequestIPAddress(httpRequest),

                };

                await _notificationService.SendPasswordResetNotification(notificationRequest);

                response.Success = true;
                response.Message = "Check your mail for your password reset link/code";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Data = null;
                return response;
            }
        }

        public async Task<ResponseHandler<string>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ResponseHandler<string> response = new ResponseHandler<string>();
            try
            {
                if (request.Password != request.ConfirmPassword)
                {
                    throw new ApplicationException("Password and password confirmation must match.");
                }

                var user = await _adminUserRepository.ResetPasswordAsync(request.Password!, request.ResetToken!);

                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Token invalid or expired";
                    response.Data = null;
                    return response;
                }


                response.Success = true;
                response.Message = "Password successfully reset";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Data = null;
                return response;
            }
        }

        public async Task<ResponseHandler<SignInResponse>> SignInAsync(SignInRequest request, HttpRequest httpRequest)
        {
            ResponseHandler<SignInResponse> response = new ();

            try
            {
                var existingUser = await _adminUserRepository.SignInAsync(new Admin
                {
                    Email = request.Email,
                    PasswordHash = request.Password
                });

                if (existingUser is null)
                {
                    response.Success = false;
                    response.Message = "Email or Password incorrect";
                }
                else
                {
                    var res = _mapper.Map<SignInResponse>(existingUser);

                    var dataUtil = _mapper.Map<UserUtilData>(existingUser);

                    res.Token = Utility.GenerateJwtToken(dataUtil, _appSettings, _appSettings.AdminAuthPolicy!);
                    res.TokenExpireAt = DateTime.Now.AddMinutes(_appSettings.JwtTokenTTLMinutees);
                    response.Message = "Admin successfully signed In";


                    if (existingUser.SendLoginNotification && existingUser.EmailConfirmed)
                    {
                        LoginNotificationRequest notificationRequest = new ()
                        {
                            RecipientName = existingUser.Firstname,
                            RecipientEmail = existingUser.Email,
                            LoginTime = DateTime.Now.ToString(),
                            LocationIp = Utility.GetRequestIPAddress(httpRequest),
                            DeviceInfo = Utility.GetDeviceInfo(httpRequest),
                            Origin = _appSettings.NotificationOrigin,
                            OriginIpAddress = Utility.GetRequestIPAddress(httpRequest)
                        };

                        await _notificationService.SendLoginNotification(notificationRequest);

                    }
                    response.Success = true;
                    response.Data = res;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return response;
            }
        }

        public Task<ResponseHandler<UpdateUserSecurityRequest>> UpdateUserSecurityAsync(Guid userId, UpdateUserSecurityRequest request)
        {
            throw new NotImplementedException();
        }

        private GetUsersOnlyResponse MappedResponse(Admin user)
        {
            var response = _mapper.Map<GetUsersOnlyResponse>(user);

            return response;
        }
    }
}
