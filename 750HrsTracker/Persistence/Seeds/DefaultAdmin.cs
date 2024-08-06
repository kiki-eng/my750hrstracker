using _750HrsTracker.Helpers;
using _750HrsTracker.Models.Admin;
using _750HrsTracker.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace _750HrsTracker.Persistence.Seeds
{
    public static class DefaultAdmin
    {
        public static async Task SeedAsync(AppDbContext context, AppSettings _appSettings)
        {
            var admins = new List<Admin>()
            {
                new Admin
                {
                    Firstname = _appSettings.AdminFirstname,
                    Lastname = _appSettings.AdminLastname,
                    Email = _appSettings.AdminEmailAddress,
                    PasswordHash = Encryption.HashPassword(Utility.GenerateRandomOtp()),
                    EmailConfirmed = true,
                }
            };

            var existingAdmins = await context.Admins.ToListAsync();
            foreach(var admin in admins)
            {
                if(!existingAdmins.Any(a => a.Email == admin.Email))
                {
                    await context.Admins.AddAsync(admin);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
