using _750HrsTracker.Models;
using _750HrsTracker.Models.Misc;

namespace _750HrsTracker.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> SignUpAsync(User user, Team team);
        Task<User> SignInAsync(User user);
        Task<User> RecoverPasswordAsync(string emailAddress);
        Task<User> ResetPasswordAsync(string newPassword, string resetToken);
        Task<User> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword);
        Task<User> VerifyEmailAsync(string verificationToken);
        Task<User> ResetEmailVerificationTokenAsync(User user);
        Task<User> UpdateUserSecurityAsync(Guid id, User user);

        Task<User> GetUserByEmailAsync(string emailAddress);
        Task<User> GetUserAsync(Guid userId);
        Task<User> GetUserAsync(Guid userId, Guid teamId);
        Task<List<UserRolesOnly>> GetUserRolesAsync(Guid userId);
        Task<User> UpdateUserAsync(Guid id, User user);

        Task<UserProfilePicture> UpdateProfilePictureAsync(UserProfilePicture picture);
        Task<UserProfilePicture> GetProfilePictureAsync(Guid userId);
        Task<UserProfilePicture> RemoveProfilePictureAsync(Guid userId);

        Task<List<Permission>> GetUserPermissionsAsync(Guid userId);
    }
}
