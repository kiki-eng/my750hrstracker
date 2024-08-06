using _750HrsTracker.Models;
using _750HrsTracker.Models.Admin;

namespace _750HrsTracker.Repositories.Interfaces
{
    public interface IAdminUserRepository : IGenericRepository<Admin>
    {
        Task<Admin> CreateAdminAsync(Admin user);
        Task<Admin> SignInAsync(Admin user);
        Task<Admin> RecoverPasswordAsync(string emailAddress);
        Task<Admin> ResetPasswordAsync(string newPassword, string resetToken);
        Task<Admin> ChangePasswordAsync(Guid adminId, string oldPassword, string newPassword);
    }
}
