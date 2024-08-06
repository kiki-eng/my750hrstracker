using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Models.ResponseWrappers;

namespace _750HrsTracker.Services.Interfaces
{
    public interface IAdminUserService
    {
        Task<ResponseHandler<GetUsersOnlyResponse>> GetUserAsync(Guid userId);
        Task<ResponseHandler<GetUsersOnlyResponse>> GetUserByTokenAsync(HttpRequest httpRequest);

        Task<ResponseHandler<UpdateUserSecurityRequest>> UpdateUserSecurityAsync(Guid userId, UpdateUserSecurityRequest request);

        Task<ResponseHandler<SignInResponse>> SignInAsync(SignInRequest request, HttpRequest httpRequest);
        Task<ResponseHandler<string>> RecoverPasswordAsync(RecoverPasswordRequest request, HttpRequest httpRequest);
        Task<ResponseHandler<string>> ResetPasswordAsync(ResetPasswordRequest request);
        Task<ResponseHandler<string>> ChangePasswordAsync(Guid userId, ChangePasswordRequest request);
    }
}
