using _750HrsTracker.Enums;
using _750HrsTracker.Helpers;
using _750HrsTracker.Models;
using _750HrsTracker.Models.JointEntities;
using _750HrsTracker.Models.Misc;
using _750HrsTracker.Persistence.Contexts;
using _750HrsTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Data;

namespace _750HrsTracker.Repositories.Implementations
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly AppSettings _appSettings;
        public UserRepository(AppDbContext context, UserManager<User> userManager, IOptionsSnapshot<AppSettings> appSettings, RoleManager<Role> roleManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _roleManager = roleManager;
        }

        public async Task<User> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword)
        {
            User user = await _context.Users.Include(m => m.UserTeams!).ThenInclude(mu => mu.Team).FirstOrDefaultAsync(m => m.Id == userId) ?? throw new KeyNotFoundException("User not found");           

            string oldPasswordHash = user.PasswordHash;
            if (!Encryption.CompareHashedPassword(oldPassword, oldPasswordHash))
            {
                throw new ApplicationException("Current password does not match");
            }

            user.PasswordHash = Encryption.HashPassword(newPassword);

            if (oldPasswordHash == user.PasswordHash)
            {
                throw new ApplicationException("New password cannot be same with old password");
            }

            user.LastPasswordResetAt = DateTime.Now;
            user.ResetToken = null;
            user.ResetTokenExpires = null;

            await _userManager.UpdateAsync(user);

            return user;
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            User user = await _context.Users.Include(m => m.UserTeams!).ThenInclude(mu => mu.Team).FirstOrDefaultAsync(m => m.Id == userId) ?? throw new KeyNotFoundException("User not found");
            return user;
        }
        public async Task<User> GetUserAsync(Guid userId, Guid teamId)
        {
            var userTeam = await _context.Team_User.Include(m => m.User).FirstOrDefaultAsync(m => m.UserId == userId && m.TeamId == teamId) ?? throw new KeyNotFoundException("User not found");
            return userTeam.User;
        }

        public async Task<User> GetUserByEmailAsync(string emailAddress)
        {
            User user = await _context.Users.Include(m => m.UserTeams!).ThenInclude(mu => mu.Team).FirstOrDefaultAsync(m => m.Email == emailAddress) ?? throw new KeyNotFoundException("User not found");
            return user;
        }

        public async Task<User> RecoverPasswordAsync(string emailAddress)
        {

            var user = await _userManager.FindByEmailAsync(emailAddress) ?? throw new ApplicationException("User with provided email address not found");


            var resetToken = Utility.GenerateRandomOtp();

            while (true)
            {
                if (_context.Users.Any(ui => ui.ResetToken == resetToken))
                {
                    resetToken = Utility.GenerateRandomOtp();
                }
                else
                {
                    break;
                }
            }

            user.ResetToken = resetToken;
            user.ResetTokenExpires = DateTime.Now.AddHours(_appSettings.ResetTokenValidHours);

            await _userManager.UpdateAsync(user);

            return user;
        }

        public async Task<User> ResetEmailVerificationTokenAsync(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(mu => mu.Id == user.Id) ?? throw new KeyNotFoundException("User not found");


            var verificationCode = Utility.GenerateRandomOtp();

            while (true)
            {
                if (_context.Users.Any(ui => ui.VerificationToken == verificationCode))
                {
                    verificationCode = Utility.GenerateRandomOtp();
                }
                else
                {
                    break;
                }
            }
            existingUser.VerificationToken = verificationCode;
            existingUser.VerificationTokenExpires = DateTime.Now.AddHours(_appSettings.VerificationTokenValidHours);

            var updated = _context.Users.Update(existingUser);

            await _context.SaveChangesAsync();

            return updated.Entity;
        }

        public async Task<User> ResetPasswordAsync(string newPassword, string resetToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ResetToken == resetToken && u.ResetTokenExpires > DateTime.Now) ?? throw new KeyNotFoundException("Invalid user/token"); 
                      

            user.PasswordHash = Encryption.HashPassword(newPassword);
            user.LastPasswordResetAt = DateTime.Now;
            user.ResetToken = null;
            user.ResetTokenExpires = null;

            await _userManager.UpdateAsync(user);

            return user;
        }

        public async Task<User> SignInAsync(User user)
        {
            var exists = await _userManager.FindByEmailAsync(user.Email) ?? throw new ApplicationException("Incorrect email or password");

            
            if (!Encryption.CompareHashedPassword(user.PasswordHash, exists.PasswordHash))
            {
                throw new ApplicationException("Incorrect email or password");
            }
            if (!exists.IsActive)
            {
                throw new ApplicationException("Cannot login now, contact admin");
            }
            return exists;
        }

        public async Task<User> SignUpAsync(User user, Team team)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(user.Email);
                if (userExists != null)
                {
                    throw new ApplicationException(message: "User with Email '" + user.Email + "' already exists");
                }

                var savedTeam = _context.Teams.Add(team);

                var verificationCode = Utility.GenerateRandomOtp();

                while (true)
                {
                    if (_context.Users.Any(ui => ui.VerificationToken == verificationCode))
                    {
                        verificationCode = Utility.GenerateRandomOtp();
                    }
                    else
                    {
                        break;
                    }
                }

                user.VerificationToken = verificationCode;
                user.VerificationTokenExpires = DateTime.Now.AddHours(_appSettings.VerificationTokenValidHours);
                user.DefaultTeamId = savedTeam.Entity.Id.ToString();
                user.IsActive = true;
                user.FirstTime = false;

              

                var newUserEntity = await _userManager.CreateAsync(user);
                var newUser = await _userManager.FindByEmailAsync(user.Email);


                List<UserRoles> userRoles = new List<UserRoles>();
                List<Role> roles = await _context.Roles.Where(r => r.Name == Roles.Admin.ToString() || r.Name == Roles.Owner.ToString()).ToListAsync();
                List<string> roleNames = new List<string>();


                foreach (var role in roles)
                {
                    UserRoles userRole = new UserRoles
                    {
                        TeamId = savedTeam.Entity.Id,
                        RoleId = role.Id,
                        UserId = newUser.Id
                    };

                    userRoles.Add(userRole);
                    roleNames.Add(role.Name);
                }
                await _context.UserRoles.AddRangeAsync(userRoles);


                savedTeam.Entity.OwnerId = newUser.Id;
                _context.Teams.Update(savedTeam.Entity);

                TeamUser teamUser = new TeamUser()
                {
                    TeamId = savedTeam.Entity.Id,
                    UserId = newUser.Id
                };


                var savedMU = _context.Team_User.Add(teamUser);


                var saved = await _context.SaveChangesAsync();

                if (saved < 1)
                {
                    return null;
                }



                return newUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<User> UpdateUserAsync(Guid id, User user)
        {
            User existingUser = await _context.Users.FirstOrDefaultAsync(m => m.Id == id) ?? throw new KeyNotFoundException("User not found");
          
            existingUser.Firstname = user.Firstname;
            existingUser.Lastname = user.Lastname;

            var updated = _context.Users.Update(existingUser); 

            return existingUser;
        }
        
        public async Task<User> UpdateUserSecurityAsync(Guid id, User user)
        {
            User existingUser = await _context.Users.FirstOrDefaultAsync(m => m.Id == id) ?? throw new KeyNotFoundException("User not found");
          
            existingUser.SendLoginNotification = user.SendLoginNotification;

            var updated = _context.Users.Update(existingUser);

            return existingUser;
        }

        public async Task<User> VerifyEmailAsync(string verificationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == verificationToken && u.VerificationTokenExpires > DateTime.Now) ?? throw new KeyNotFoundException("Invalid user/token");

            user.EmailConfirmed = true;
            user.VerificationToken = null;
            user.VerificationTokenExpires = null;
            user.ModifiedAt = DateTime.Now;

            var updated = _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return updated.Entity;
        }

        public async Task<List<UserRolesOnly>> GetUserRolesAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(mu => mu.Id.Equals(userId)) ?? throw new KeyNotFoundException("User not found");
            var userRoles = _context.UserRoles.Where(mur => mur.UserId == userId && mur.TeamId == Guid.Parse(user.DefaultTeamId!)).ToList();

            List<UserRolesOnly> roles = new();
            foreach (var r in userRoles)
            {
                var role = await _roleManager.FindByIdAsync(r.RoleId.ToString());
                UserRolesOnly rolesOnly = new ()
                {
                    RoleName = role.Name,
                    RoleId = role.Id
                };

                roles.Add(rolesOnly);
            }

            return roles;
        }
    }
}
