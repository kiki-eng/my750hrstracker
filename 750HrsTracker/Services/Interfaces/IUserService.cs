using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Filters;
using _750HrsTracker.Models.ResponseWrappers;

namespace _750HrsTracker.Services.Interfaces
{
    public interface IUserService
    {

        Task<ResponseHandler<GetUsersOnlyResponse>> GetUserAsync(Guid userId);
        Task<ResponseHandler<GetUsersOnlyResponse>> GetUserByTokenAsync(HttpRequest httpRequest);

        Task<ResponseHandler<UpdateUserSecurityRequest>> UpdateUserSecurityAsync(Guid userId, UpdateUserSecurityRequest request);
        Task<ResponseHandler<GetUsersOnlyResponse>> UpdatetUserProfileAsync(Guid userId, UpdateUserRequest request);



        Task<ResponseHandler<SignInResponse>> SignInAsync(SignInRequest request, HttpRequest httpRequest);
        Task<ResponseHandler<string>> SignUpAsync(SignUpRequest request, HttpRequest httpRequest, bool fromAdmin = false);


        Task<ResponseHandler<string>> RecoverPasswordAsync(RecoverPasswordRequest request, HttpRequest httpRequest);
        Task<ResponseHandler<string>> ResetPasswordAsync(ResetPasswordRequest request);
        Task<ResponseHandler<string>> ChangePasswordAsync(Guid userId, ChangePasswordRequest request);
        Task<ResponseHandler<string>> VerifyEmailAsync(VerifyEmailRequest request);
        Task<ResponseHandler<string>> ResendVerifyEmailAsync(ResendVerifyEmailRequest request, HttpRequest httpRequest);

        Task<ResponseHandler<UpdateProfilePictureRequest>> UpdatetUserProfilePictureAsync(Guid userId, UpdateProfilePictureRequest request);
        Task<ResponseHandler<Base64FileModel>> GetUserProfilePictureAsync(Guid userId);
        Task<ResponseHandler<string>> RemoveProfilePictureAsync(Guid userId);

        Task<ResponseHandler<GetUserPermissionsResponse>> GetUserPermissionsAsync(HttpRequest httpRequest);
    }
}
