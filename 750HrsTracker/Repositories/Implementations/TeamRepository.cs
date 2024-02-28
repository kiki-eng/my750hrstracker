using _750HrsTracker.Enums;
using _750HrsTracker.Helpers;
using _750HrsTracker.Helpers.Constants;
using _750HrsTracker.Models;
using _750HrsTracker.Models.JointEntities;
using _750HrsTracker.Persistence.Contexts;
using _750HrsTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using System.Data;
using System.Security.Claims;

namespace _750HrsTracker.Repositories.Implementations
{
    public class TeamRepository : ITeamRepository
    {
        private readonly AppDbContext _context;
        private readonly AppSettings _appSettings;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public TeamRepository(AppDbContext context, IOptionsSnapshot<AppSettings> appSettings, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _roleManager = roleManager;

        }
        public async Task<Role> AddTeamRoleAsync(Guid teamId, string roleName, Guid currentUserId, List<Permission> permissions)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(m => m.Id == teamId) ?? throw new KeyNotFoundException("Invalid team");
            var currentUser = await _userManager.FindByIdAsync(currentUserId.ToString());
        
            var role = await _context.Roles.AddAsync(new Role
            {
                Name = roleName,
                Slug = Utility.GenerateSlug(roleName),
                TeamId = team.Id,
                RoleType = RoleType.team,
                CreatorId = currentUser.Id.ToString(),
                CreatedBy = $"{currentUser.Firstname} {currentUser.Lastname}",
                CreatedAt = DateTime.Now,
                ModifiedBy = $"{currentUser.Firstname} {currentUser.Lastname}",
                ModifiedById = currentUser.Id.ToString(),
                Default = false
            });

            await _context.SaveChangesAsync();

            var newRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == role.Entity.Id);


            var allClaims = await _roleManager.GetClaimsAsync(newRole!);
            foreach (var permission in permissions)
            {
                var perm = _context.Permissions.FirstOrDefault(p => p.Id == permission.Id);
                if (perm != null)
                {
                    if (!allClaims.Any(a => a.Type == PermissionConstants.Permission && a.Value == perm.Value))
                        await _roleManager.AddClaimAsync(newRole!, new Claim(PermissionConstants.Permission, perm.Value!));
                }
            }

            return newRole!;
        }

        public async Task<List<Role>> GetTeamRolesAsync(Guid teamId)
        {
            var roles = new List<Role>();

            var team = await _context.Teams.FirstOrDefaultAsync(m => m.Id == teamId) ?? throw new KeyNotFoundException("Invalid team");

            var defaultRoles = await _context.Roles.Where(r => r.Name.ToLower() != Roles.Owner.ToString().ToLower() && r.Default).ToListAsync();

            var customRoles = await _context.Roles.Where(r => r.TeamId.Equals(team.Id) && !r.Default).ToListAsync();

            roles = defaultRoles.Union(customRoles).ToList();

            return roles;
        }

        public async Task<Role> UpdateTeamRoleAsync(Guid teamId, Guid roleId, Guid currentUserId, Role role)
        {

            var team = await _context.Teams.FirstOrDefaultAsync(m => m.Id == teamId) ?? throw new KeyNotFoundException("Invalid team");

            var existingRole = await _context.Roles.FirstOrDefaultAsync(mr => mr.Id == roleId && mr.TeamId.Equals(team.Id)) ?? throw new KeyNotFoundException("Invalid role");
            var currentUser = await _userManager.FindByIdAsync(currentUserId.ToString());

            existingRole.Name = role.Name;
            existingRole.Slug = Utility.GenerateSlug(role.Name);
            existingRole.ModifiedAt = DateTime.Now;
            existingRole.ModifiedBy = $"{currentUser.Firstname} {currentUser.Lastname}";
            existingRole.ModifiedById = currentUser.Id.ToString();

            var updated = _context.Roles.Update(existingRole);
            await _context.SaveChangesAsync();

            return updated.Entity;
        }

        public async Task<Role> DeleteRoleAsync(Guid teamId, Guid roleId)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(m => m.Id == teamId) ?? throw new KeyNotFoundException("Invalid team");

            var existingRole = await _context.Roles.FirstOrDefaultAsync(mr => mr.Id == roleId && mr.TeamId.Equals(team.Id)) ?? throw new KeyNotFoundException("Invalid role");

            var userInRole = await _context.UserRoles.AnyAsync(ur => ur.RoleId == existingRole.Id && ur.TeamId == team.Id);

            if (userInRole)
            {
                throw new ApplicationException("Cannot delete role. Users are still attached to role");
            }

            var deleted = _context.Roles.Remove(existingRole);
            await _context.SaveChangesAsync();
            return deleted.Entity;

        }

        public async Task<Role> UpdateRolePermissionsAsync(Guid teamId, Guid roleId, Guid currentUserId, List<Permission> permissions)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(m => m.Id == teamId) ?? throw new KeyNotFoundException("Invalid team");

            var existingRole = await _context.Roles.FirstOrDefaultAsync(mr => mr.Id == roleId && mr.TeamId.Equals(team.Id)) ?? throw new KeyNotFoundException("Invalid role");

            var currentUser = await _userManager.FindByIdAsync(currentUserId.ToString());
            var allClaims = await _roleManager.GetClaimsAsync(existingRole);


            foreach (var permission in permissions)
            {
                var perm = _context.Permissions.FirstOrDefault(p => p.Id.Equals(permission.Id) && p.Value == permission.Value);
                if (perm != null)
                {
                    if (!allClaims.Any(a => a.Type == PermissionConstants.Permission && a.Value == perm.Value))
                        await _roleManager.AddClaimAsync(existingRole, new Claim(PermissionConstants.Permission, perm.Value!));
                }
            }

            existingRole.ModifiedAt = DateTime.Now;
            existingRole.ModifiedBy = $"{currentUser.Firstname} {currentUser.Lastname}";
            existingRole.ModifiedById = currentUser.Id.ToString();

            var updated = _context.Roles.Update(existingRole);
            await _context.SaveChangesAsync();

            return updated.Entity;
        }

        // === user invitation start === //
        public async Task<UserInvitation> InviteUserAsync(Guid teamId, Guid userId, string inviteeEmail, Guid roleId)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(m => m.Id == teamId) ?? throw new KeyNotFoundException("Unknown team");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId && u.DefaultTeamId == team.Id.ToString()) ?? throw new KeyNotFoundException("Invalid inviter");

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId) ?? throw new KeyNotFoundException("Invalid role selected");

            var existingInvitation = await _context.UserInvitations.FirstOrDefaultAsync(ui => ui.Email == inviteeEmail && ui.TeamId == team.Id);
            var invitationCode = Utility.GenerateRandomOtp();

            while (true)
            {
                if (_context.UserInvitations.Any(ui => ui.Code == invitationCode))
                {
                    invitationCode = Utility.GenerateRandomOtp();
                }
                else
                {
                    break;
                }
            }

            if (existingInvitation != null)
            {
                existingInvitation.Code = invitationCode;
                existingInvitation.ExpiresAt = DateTime.Now.AddMinutes(_appSettings.InvitationTokenExpiresMinutes);
                existingInvitation.InviterId = user.Id;
                existingInvitation.RoleName = role.Name;
                existingInvitation.RoleId = role.Id.ToString();
                existingInvitation.InviterEmail = user.Email;
                existingInvitation.InviterName = user.Firstname;

                var updated = _context.UserInvitations.Update(existingInvitation);
                await _context.SaveChangesAsync();


                return updated.Entity;
            }

            var userInvitation = new UserInvitation()
            {
                Email = inviteeEmail,
                Code = invitationCode,
                InviterId = user.Id,
                TeamId = team.Id,
                TeamName = team.Name,
                ExpiresAt = DateTime.Now.AddMinutes(_appSettings.InvitationTokenExpiresMinutes),
                RoleName = role.Name,
                RoleId = role.Id.ToString(),
                InviterEmail = user.Email,
                InviterName = user.Firstname,
            };

            var savedInvitation = _context.UserInvitations.Add(userInvitation);
            await _context.SaveChangesAsync();

            return savedInvitation.Entity;           
        }

        public async Task<UserInvitation> ValidateInvitationAsync(string inviteeEmail, string invitationCode)
        {
            var invitationDetails = await _context.UserInvitations.FirstOrDefaultAsync(i => i.Email == inviteeEmail && i.Code == invitationCode) ?? throw new ApplicationException("Invalid invitation");


            if (invitationDetails.ExpiresAt < DateTime.Now)
            {
                throw new ApplicationException("Invitation expired. Contact your inviter to get a new invitation link");
            }

            return invitationDetails;
        }

        public async Task<bool> IsInvitedUserConfirmed(string inviteeEmail)
        {
            var invitationDetails = await _context.UserInvitations.FirstOrDefaultAsync(i => i.Email == inviteeEmail);

            return await _context.Users.AnyAsync(u => u.Email == invitationDetails!.Email && u.EmailConfirmed);
           
        }
        public async Task<User> CreateInvitedUserAsync(User user, string invitationCode)
        {
            EntityEntry<User> updatedUser = null!;

            // validate code 
            var invitationDetails = await _context.UserInvitations.FirstOrDefaultAsync(u => u.Email == user.Email && u.Code == invitationCode) ?? throw new ApplicationException("Invalid Invitation");

            if (invitationDetails.ExpiresAt < DateTime.Now)
            {
                throw new ApplicationException("Invitation code expired. Contact your inviter to get a new invitation link");
            }

            var team = await _context.Teams.FirstOrDefaultAsync(m => m.Id == invitationDetails.TeamId) ?? throw new ApplicationException("Invalid team invitation");

            UserRoles userRoles = new()
            {
                TeamId = team.Id,
                RoleId = Guid.Parse(invitationDetails.RoleId!)
            };

            TeamUser userTeam = new()
            {
                TeamId = team.Id
            };


            var userExists = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (userExists != null)
            {
                //add user to the new team if he already exist on the system    

                userTeam.UserId = userExists.Id;
                userRoles.UserId = userExists.Id;

            }
            else
            {
                user.EmailConfirmed = true;
                user.IsActive = true;
                user.FirstTime = false;
                user.DefaultTeamId = team.Id.ToString();

                var savedUser = await _userManager.CreateAsync(user);
                var newUser = await _userManager.FindByEmailAsync(user.Email);

                userTeam.UserId = newUser.Id;
                userRoles.UserId = newUser.Id;
            }


            await _context.Team_User.AddAsync(userTeam);
            await _context.UserRoles.AddAsync(userRoles);
            _context.UserInvitations.Remove(invitationDetails);
            await _context.SaveChangesAsync();

            return updatedUser.Entity;
        }

        public async Task<List<UserInvitation>> GetPendingUserInvitationsAsync(Guid teamId)
        {
            var invitations = await _context.UserInvitations.Where(ui => ui.TeamId == teamId).ToListAsync();

            return invitations!;
        }

        // === user invitation end === //
    }
}
