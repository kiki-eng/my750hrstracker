using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Enums;
using _750HrsTracker.Extensions;
using _750HrsTracker.Helpers;
using _750HrsTracker.Helpers.Constants;
using _750HrsTracker.Models;
using _750HrsTracker.Models.ActivityLogModels;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Repositories.Implementations;
using _750HrsTracker.Repositories.Interfaces;
using _750HrsTracker.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net.Mail;

namespace _750HrsTracker.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly INotificationService _notificationService;
        private readonly ITeamSubscriptionRepository _teamSubscriptionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IOptionsSnapshot<AppSettings> appSettings, INotificationService notificationService, 
            IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger, ITeamSubscriptionRepository teamSubscriptionRepository)
        {
            _appSettings = appSettings.Value;
            _notificationService = notificationService;
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;   
            _teamSubscriptionRepository = teamSubscriptionRepository;
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

        public async Task<ResponseHandler<GetUsersOnlyResponse>> GetUserAsync(Guid userId)
        {
            try
            {
                ResponseHandler<GetUsersOnlyResponse> response = new ResponseHandler<GetUsersOnlyResponse>();


                User user = await _userRepository.GetUserAsync(userId) ?? throw new KeyNotFoundException("user not found");

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

                var  details = Utility.GetUserIdFromToken(httpRequest) ?? throw new ApplicationException("Invalid user token");

                string userId = details.Item1;

                User user = await _userRepository.GetUserAsync(Guid.Parse(userId)) ?? throw new KeyNotFoundException("user not found");
                var responseData = MappedResponse(user);

                var userProfilePic = await _userRepository.GetProfilePictureAsync(user.Id);

                if(userProfilePic != null)
                {
                    byte[] fileBytes = await Storage.DownloadDocumentAsStream(_appSettings, userProfilePic.RemoteDirectoryName!);

                    Base64FileModel fileModel = new()
                    {
                        ContentType = Utility.GetMimeType(userProfilePic.DocumentName!),
                        FileExtension = Path.GetExtension(userProfilePic.DocumentName!),
                        Data = Convert.ToBase64String(fileBytes),
                        FileName = userProfilePic.DocumentName,
                    };

                    responseData.ProfilePic = fileModel;
                }

                var subscriptionData = await _teamSubscriptionRepository.GetWthSubscription(Guid.Parse(user.DefaultTeamId!));
                if(subscriptionData != null)
                {
                    responseData.Subscription = new SubscriptionData
                    {
                        IsActive = subscriptionData.EndDate > DateTime.Now,
                        StartDate = (DateTime)subscriptionData.StartDate!,
                        EndDate = (DateTime)subscriptionData.EndDate!,
                        Name = subscriptionData.Subscription?.Name
                    };
                }

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

        public async Task<ResponseHandler<Base64FileModel>> GetUserProfilePictureAsync(Guid userId)
        {
            ResponseHandler<Base64FileModel> response = new();
            Base64FileModel responseData = new();

            var userProfilePic = await _userRepository.GetProfilePictureAsync(userId);

            if (userProfilePic != null)
            {
                byte[] fileBytes = await Storage.DownloadDocumentAsStream(_appSettings, userProfilePic.RemoteDirectoryName!);

                Base64FileModel fileModel = new()
                {
                    ContentType = Utility.GetMimeType(userProfilePic.DocumentName!),
                    FileExtension = Path.GetExtension(userProfilePic.DocumentName!),
                    Data = Convert.ToBase64String(fileBytes),
                    FileName = userProfilePic.DocumentName,
                };

                responseData = fileModel;
            }
            
            response.Success = true;
            response.Message = "User profile picture retrieved successfully";
            response.Data = responseData;
            return response;
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

        public async Task<ResponseHandler<string>> RemoveProfilePictureAsync(Guid userId)
        {
           var deleted = await _userRepository.RemoveProfilePictureAsync(userId);

            return new ResponseHandler<string>()
            {
                Success = true,
                Message = "Profile pic successfully removed"
            };
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

                    res.Token = res.EmailConfirmed ? Utility.GenerateJwtToken(_mapper.Map<UserUtilData>(existingUser), _appSettings, _appSettings.UserAuthPolicy!) : "";
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
                    UserName = $"{address.User}",
                    PhoneNumber = request.PhoneNumber,
                    PasswordHash = Encryption.HashPassword(request.Password!),
                };

                if (fromAdmin)
                {
                    newUser.EmailConfirmed = true;
                    newUser.SendLoginNotification = false;
                }


                var savedUser = await _userRepository.SignUpAsync(newUser, newTeam);

                List<GetUsersOnlyResponse> merchantUsers = new List<GetUsersOnlyResponse>();              

                NewUserNotificationRequest newUserNotificationRequest = new NewUserNotificationRequest
                {
                    RecipientName = _appSettings.SystemNotificationReceiverName,
                    RecipientEmail = _appSettings.SystemNotificationReceiverEmail,
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

        public async Task<ResponseHandler<GetUsersOnlyResponse>> UpdatetUserProfileAsync(Guid userId, UpdateUserRequest request)
        {
            try
            {
                ResponseHandler<GetUsersOnlyResponse> response = new ResponseHandler<GetUsersOnlyResponse>();

                var user = await _userRepository.UpdateUserAsync(userId, new User { Firstname = request.Firstname, Lastname = request.Lastname, PhoneNumber = request.PhoneNumber});

                response.Success = true;
                response.Message = "User updated successfully";
                response.Data = _mapper.Map<GetUsersOnlyResponse>(user);
                return response;
            }
            catch
            {
                throw;
            }
        }
        
        public async Task<ResponseHandler<UpdateProfilePictureRequest>> UpdatetUserProfilePictureAsync(Guid userId, UpdateProfilePictureRequest request)
        {
            try
            {
                ResponseHandler<UpdateProfilePictureRequest> response = new ResponseHandler<UpdateProfilePictureRequest>();

                var user = await _userRepository.GetUserAsync(userId) ?? throw new ApplicationException("User not found");
                // Convert Base64 string to byte array
                Base64FormFile file = new Base64FormFile();

                if (request.ProfilePicture != null)
                {
                    var profilePic = request.ProfilePicture;
                    byte[] fileBytes = Convert.FromBase64String(profilePic.Data!);
                    var extension = Path.GetExtension(profilePic.FileName!).ToLower();

                    if (string.IsNullOrEmpty(extension))
                        throw new ApplicationException("Invalid file name. File name must contain the file extension");
                    profilePic.FileName = $"{userId}{extension}";
                    file = new Base64FormFile(profilePic.FileName!, profilePic.ContentType!, fileBytes);                   
                }


                if (request.ProfilePicture != null)
                {
                    var fileType = file.ContentType;

                    var extension = Path.GetExtension(file.FileName).ToLower();

                    var fileName = user.Id.ToString().ToLower() + extension;

                    var documentUploadResponse = await Storage.UploadDocumentAsync(_appSettings, file, DocumentFor.ProfilePicture, fileName);

                    if (documentUploadResponse != null)
                    {
                        UserProfilePicture userProfilePicture = new()
                        {
                            UserId = user.Id,
                            RemoteDirectoryName = documentUploadResponse.DirectoryName,
                            DocumentPath = documentUploadResponse.FileAbsoluteUri,
                            DocumentExtension = extension,
                            DocumentName = fileName,
                            DocumentType = fileType,
                            
                        };
                        await _userRepository.UpdateProfilePictureAsync(userProfilePicture!);
                    }
                }

                response.Success = true;
                response.Message = "User profile pic updated successfully";
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
        public async Task<ResponseHandler<GetUserPermissionsResponse>> GetUserPermissionsAsync(HttpRequest httpRequest)
        {
            try
            {
                var response = new ResponseHandler<GetUserPermissionsResponse>();


                var details = Utility.GetUserIdFromToken(httpRequest) ?? throw new ApplicationException("Invalid user token");

                string userId = details.Item1;
                User user = await _userRepository.GetUserAsync(Guid.Parse(userId)) ?? throw new KeyNotFoundException("user not found");

                var userPermissions = await _userRepository.GetUserPermissionsAsync(Guid.Parse(userId));
                GetUserPermissionsResponse responsedata = new()
                {
                    TeamId = Guid.Parse(user.DefaultTeamId!),
                    UserId = Guid.Parse(userId),
                    Permissions = userPermissions.Select(p => _mapper.Map<GetPermissionResponse>(p)).ToList()
                };

                response.Success = true;
                response.Message = "User permissions retrievd successfully";
                response.Data = responsedata;

                return response;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        private GetUsersOnlyResponse MappedResponse(User user)
        {
            var response = _mapper.Map<GetUsersOnlyResponse>(user);

            if (user.Roles != null && user.Roles!.Count > 0)
            {
                response.Roles = user.Roles.Select(r => _mapper.Map<GetRolesOnlyResponse>(r)).ToList();
            }

            return response;
        }
    }
}
