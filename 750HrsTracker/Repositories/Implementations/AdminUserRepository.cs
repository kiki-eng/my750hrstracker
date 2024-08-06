using _750HrsTracker.Helpers;
using _750HrsTracker.Models.Admin;
using _750HrsTracker.Persistence.Contexts;
using _750HrsTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;

namespace _750HrsTracker.Repositories.Implementations
{
    public class AdminUserRepository : GenericRepository<Admin>, IAdminUserRepository
    {
        private readonly AppDbContext _context;
        private readonly AppSettings _appSettings;
        public AdminUserRepository(AppDbContext context, IOptionsSnapshot<AppSettings> appSettings) : base(context)
        {
            _context = context;
            _appSettings = appSettings.Value;

        }

        public async Task<Admin> ChangePasswordAsync(Guid adminId, string oldPassword, string newPassword)
        {
            Admin admin = await _context.Admins.FirstOrDefaultAsync(m => m.Id == adminId) ?? throw new KeyNotFoundException("Admin not found");

            string oldPasswordHash = admin.PasswordHash;
            if (!Encryption.CompareHashedPassword(oldPassword, oldPasswordHash))
            {
                throw new ApplicationException("Current password does not match");
            }

            admin.PasswordHash = Encryption.HashPassword(newPassword);

            if (oldPasswordHash == admin.PasswordHash)
            {
                throw new ApplicationException("New password cannot be same with old password");
            }

            admin.LastPasswordResetAt = DateTime.Now;
            admin.ResetToken = null;
            admin.ResetTokenExpires = null;

            var updated = _context.Admins.Update(admin);
            await _context.SaveChangesAsync();

            return updated.Entity;
        }

        public async Task<Admin> CreateAdminAsync(Admin admin)
        {
            var added = await _context.Admins.AddAsync(admin);

            await _context.SaveChangesAsync();

            return added.Entity;
        }

        public async Task<Admin> RecoverPasswordAsync(string emailAddress)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == emailAddress) ?? throw new ApplicationException("User with provided email address not found");

            var resetToken = Utility.GenerateRandomOtp();

            while (true)
            {
                if (_context.Admins.Any(ui => ui.ResetToken == resetToken))
                {
                    resetToken = Utility.GenerateRandomOtp();
                }
                else
                {
                    break;
                }
            }

            admin.ResetToken = resetToken;
            admin.ResetTokenExpires = DateTime.Now.AddHours(_appSettings.ResetTokenValidHours);

            var updated = _context.Admins.Update(admin);
            await _context.SaveChangesAsync();

            return updated.Entity;
        }

        public async Task<Admin> ResetPasswordAsync(string newPassword, string resetToken)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(u => u.ResetToken == resetToken && u.ResetTokenExpires > DateTime.Now) ?? throw new KeyNotFoundException("Invalid user/token");

            admin.PasswordHash = Encryption.HashPassword(newPassword);
            admin.LastPasswordResetAt = DateTime.Now;
            admin.ResetToken = null;
            admin.ResetTokenExpires = null;

            var updated = _context.Admins.Update(admin);
            await _context.SaveChangesAsync();

            return updated.Entity;
        }

        public async Task<Admin> SignInAsync(Admin admin)
        {
            var exists = await _context.Admins.FirstOrDefaultAsync(a => a.Email == admin.Email) ?? throw new ApplicationException("User with provided email address not found");

            if (!Encryption.CompareHashedPassword(admin.PasswordHash, exists.PasswordHash))
            {
                throw new ApplicationException("Incorrect email or password");
            }
            if (!exists.IsActive)
            {
                throw new ApplicationException("Cannot login now, contact admin");
            }
            return exists;
        }
    }
}
