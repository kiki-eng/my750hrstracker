using _750HrsTracker.Helpers.Constants;
using _750HrsTracker.Models;
using _750HrsTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace _750HrsTracker.PermissionManagement
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {

        UserManager<User> _userManager;
        RoleManager<Role> _roleManager;
        IServiceProvider _serviceProvider;
        ILogger _logger;

        public PermissionAuthorizationHandler(UserManager<User> userManager, RoleManager<Role> roleManager, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = loggerFactory.CreateLogger("PMW");
            _serviceProvider = serviceProvider;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            _logger.LogInformation(context.User.ToString());

            if (context.User == null)
            {
                return;
            }

            if (context.User.HasClaim(c => c.Type == "Id"))
            {
                var userId = context.User.Claims.First(x => x.Type == "Id").Value;

                // Get all the roles the user belongs to and check if any of the roles has the permission required
                // for the authorization to succeed.
                var user = await _userManager.FindByIdAsync(userId);

                var userRepository = _serviceProvider.GetRequiredService<IUserRepository>();

                var userRoles = await userRepository.GetUserRolesAsync(user.Id);

                var userRoleNames = userRoles.Select(r => r.RoleName).ToList();

                var roles = _roleManager.Roles.Where(x => userRoleNames.Contains(x.Name)).ToList();

                foreach (var role in roles)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);

                    var permissionss = roleClaims.Where(x => x.Type == PermissionConstants.Permission &&
                                                               x.Value == requirement.Permission &&
                                                               x.Issuer == "LOCAL AUTHORITY");
                    if (permissionss.Any())
                    {
                        context.Succeed(requirement);
                        return;
                    }

                }
            }
        }
    }
}
