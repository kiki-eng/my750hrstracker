using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Helpers;
using _750HrsTracker.Models;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Repositories.Interfaces;
using _750HrsTracker.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace _750HrsTracker.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly INotificationService _notificationService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IOptionsSnapshot<AppSettings> appSettings, INotificationService notificationService, IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
        {
            _appSettings = appSettings.Value;
            _notificationService = notificationService;
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;   
        }

        public async Task<ResponseHandler<string>> ChangePasswordAsync(Guid userId, ChangePasswordRequest request)
        {
            try
            {
                ResponseHandler<string> response = new ResponseHandler<string>();

                var user = await _userRepository.ChangePasswordAsync(userId, request.OldPassword!, request.NewPassword!);

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

        public async Task<ResponseHandler<GetUserResponse>> GetUserAsync(Guid userId)
        {
            try
            {
                ResponseHandler<GetUserResponse> response = new ResponseHandler<GetUserResponse>();


                User user = await _userRepository.GetUserAsync(userId) ?? throw new KeyNotFoundException("user not found");

                response.Success = true;
                response.Message = "User retrieved successfully";
                response.Data = _mapper.Map<GetUserResponse>(user);
                return response;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseHandler<GetUserResponse>> GetUserByTokenAsync(HttpRequest httpRequest)
        {
            try
            {
                ResponseHandler<GetUserResponse> response = new ResponseHandler<GetUserResponse>();

                string userId = Utility.GetUserIdFromToken(httpRequest) ?? throw new ApplicationException("Invalid user token");


                User user = await _userRepository.GetUserAsync(Guid.Parse(userId)) ?? throw new KeyNotFoundException("user not found");
                var responseData = _mapper.Map<GetUserResponse>(user);
                responseData.DefaulTeamId = user.DefaultTeamId;
                response.Success = true;
                response.Message = "User retrieved successfully";
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

                var merchantUser = await _userRepository.RecoverPasswordAsync(request.EmailAddress!);

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
                response.Message = "Check your mail for your password reset link";
                response.Data = null;
                _logger.LogInformation(response.Message);
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

        public async Task<ResponseHandler<string>> ResendVerifyEmailAsync(ResendVerifyEmailRequest request, HttpRequest httpRequest)
        {
            try
            {
                ResponseHandler<string> response = new ResponseHandler<string>();

                var user = await _userRepository.GetUserByEmailAsync(request.EmailAddress!) ?? throw new KeyNotFoundException("user not found");


                if (user.VerificationTokenExpires < DateTime.Now || user.VerificationTokenExpires == null)
                {
                    var resetUser = await _userRepository.ResetEmailVerificationTokenAsync(user);
                    user.VerificationToken = resetUser.VerificationToken;
                    user.VerificationTokenExpires = resetUser.VerificationTokenExpires;
                }
                string toBeEncoded = $"{user.Email}|{user.VerificationToken}";

                EmailVerificationNotificationRequest notificationRequest = new EmailVerificationNotificationRequest
                {
                    RecipientName = user.Firstname,
                    RecipientEmail = user.Email,
                    VerifyEmailToken = user.VerificationToken,
                    Origin = _appSettings.NotificationOrigin,
                    OriginIpAddress = Utility.GetRequestIPAddress(httpRequest),
                };

                await _notificationService.SendEmailVerificationNotification(notificationRequest);


                response.Success = true;
                response.Message = "Email verification token resent successfully";
                response.Data = null;
                return response;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseHandler<string>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ResponseHandler<string> response = new ResponseHandler<string>();
            try
            { 
                if(request.Password != request.ConfirmPassword)
                {
                    throw new ApplicationException("Password and password confirmation must match.");
                }

                var user = await _userRepository.ResetPasswordAsync(request.Password!, request.ResetToken!);

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
            ResponseHandler<SignInResponse> response = new ResponseHandler<SignInResponse>();

            try
            {
                User user = new User
                {
                    Email = request.Email,
                    PasswordHash = request.Password
                };
                var existingUser = await _userRepository.SignInAsync(user);

                if (existingUser is null)
                {
                    response.Success = false;
                    response.Message = "Email or Password incorrect";
                }
                else
                {
                    var res = _mapper.Map<SignInResponse>(existingUser);

                    res.Token = res.EmailConfirmed ? Utility.GenerateJwtToken(_mapper.Map<UserUtilData>(existingUser), _appSettings) : "";
                    res.TokenExpireAt = res.EmailConfirmed ? DateTime.Now.AddMinutes(_appSettings.JwtTokenTTLMinutees) : DateTime.Now;
                    response.Message = res.EmailConfirmed ? "User successfully signed In" : "User email verification not completed. Please verify your email to continue";


                    if (existingUser.SendLoginNotification && existingUser.EmailConfirmed)
                    {
                        LoginNotificationRequest notificationRequest = new LoginNotificationRequest
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

        public async Task<ResponseHandler<string>> SignUpAsync(SignUpRequest request, HttpRequest httpRequest, bool fromAdmin = false)
        {
            ResponseHandler<string> response = new ResponseHandler<string>();

            try
            {

                MailAddress address = new MailAddress(request.Email!);

                Team newTeam = new Team()
                {
                    Name = request.Lastname + " " + request.Firstname

                };
                User newUser = new User()
                {
                    Firstname = request.Firstname,
                    Lastname = request.Lastname,
                    Email = request.Email,
                    UserName = $"{address.User}_{Utility.RandomString(4)}",
                    PasswordHash = Encryption.HashPassword(request.Password!),
                };

                if (fromAdmin)
                {
                    newUser.EmailConfirmed = true;
                    newUser.SendLoginNotification = false;
                }


                var savedUser = await _userRepository.SignUpAsync(newUser, newTeam);

                List<GetUserResponse> merchantUsers = new List<GetUserResponse>();              

                NewUserNotificationRequest newUserNotificationRequest = new NewUserNotificationRequest
                {
                    RecipientName = "NA",
                    RecipientEmail = _appSettings.SystemNotificationReceiver,
                    CreationDate = savedUser.CreatedAt,
                    TeamName = newTeam.Name,
                    Link = "",
                    Origin = _appSettings.NotificationOrigin,
                    OriginIpAddress = Utility.GetRequestIPAddress(httpRequest),
                };

                if (!savedUser.EmailConfirmed)
                {

                    EmailVerificationNotificationRequest notificationRequest = new EmailVerificationNotificationRequest
                    {
                        RecipientName = savedUser.Firstname,
                        RecipientEmail = savedUser.Email,
                        VerifyEmailToken = savedUser.VerificationToken,
                        Origin = _appSettings.NotificationOrigin,
                        OriginIpAddress = Utility.GetRequestIPAddress(httpRequest),
                    };

                    await _notificationService.SendEmailVerificationNotification(notificationRequest);

                }
                await _notificationService.NewAccountNotificationToAdmin(newUserNotificationRequest);

                if (savedUser == null)
                {
                    response.Success = false;
                    response.Message = "Unable to sign up new user";
                    response.Data = null;
                    return response;
                }

                response.Success = true;
                response.Message = "User Successfully Registered";

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return response;
            }
        }

        public async Task<ResponseHandler<GetUserResponse>> UpdatetUserProfileAsync(Guid userId, UpdateUserRequest request)
        {
            try
            {
                ResponseHandler<GetUserResponse> response = new ResponseHandler<GetUserResponse>();

                var user = await _userRepository.UpdateUserAsync(userId, _mapper.Map<User>(request));

                response.Success = true;
                response.Message = "User updated successfully";
                response.Data = _mapper.Map<GetUserResponse>(user);
                return response;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseHandler<UpdateUserSecurityRequest>> UpdateUserSecurityAsync(Guid userId, UpdateUserSecurityRequest request)
        {
            try
            {
                ResponseHandler<UpdateUserSecurityRequest> response = new ResponseHandler<UpdateUserSecurityRequest>();

                var user = await _userRepository.UpdateUserSecurityAsync(userId, _mapper.Map<User>(request));

                response.Success = true;
                response.Message = "User updated successfully";
                response.Data = _mapper.Map<UpdateUserSecurityRequest>(user);
                return response;
            }
            catch 
            {
                throw;
            }
        }

        public async Task<ResponseHandler<string>> VerifyEmailAsync(VerifyEmailRequest request)
        {
            ResponseHandler<string> response = new ResponseHandler<string>();

            var merchantUser = await _userRepository.VerifyEmailAsync(request.VerificationToken!);

            if (merchantUser == null)
            {
                response.Success = false;
                response.Message = "Token not found or expired";
                response.Data = null;
                return response;
            }


            response.Success = true;
            response.Message = "Email verified successfully";
            response.Data = null;
            return response;
        }
    }
}
