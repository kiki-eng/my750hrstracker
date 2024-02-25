using _750HrsTracker.Enums;
using _750HrsTracker.Helpers.Constants;
using _750HrsTracker.Models;
using _750HrsTracker.Persistence.Contexts;
using _750HrsTracker.Repositories.Implementations;
using _750HrsTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace _750HrsTracker.Persistence.Seeds
{
    public static class DefaultPermission
    {
        public static async Task SeedPermissionForRoleAsync(RoleManager<Role> roleManager, AppDbContext context)
        {
            //Seed Default Permission
            List<Permission> permissions = new List<Permission>()
            {
                //Dashboard
                new Permission { Module = "Dashboard", Name = "View"}, 
                
                //Row
                new Permission { Module = "Role", Name = "Create"},
                new Permission { Module = "Role", Name = "View"},
                new Permission { Module = "Role", Name = "Update"},
                new Permission { Module = "Role", Name = "Delete"},

                //User
                new Permission { Module = "Users", Name = "Create"},
                new Permission { Module = "Users", Name = "View"},
                new Permission { Module = "Users", Name = "Update"},
                new Permission { Module = "Users", Name = "Delete"},
                new Permission { Module = "Users", Name = "Switch"},
            };


            PermissionRepository permissionsRepository = new PermissionRepository(context);

            foreach (var permission in permissions)
            {
                var exists = context.Permissions.Any(p => p.Module!.ToLower() == permission.Module!.ToLower() && p.Name!.ToLower() == permission.Name!.ToLower());
                if (!exists)
                {
                    await permissionsRepository.AddAsync(permission);
                }
            }

            //seed role permissions
            await SeedInitialRolePermission(context, roleManager);
        }
        private static async Task SeedInitialRolePermission(AppDbContext context, RoleManager<Role> roleManager)
        {
            var rolesRepository = new RoleRepository(context);
            var basicPermission = new List<string>
            {
                "Permission.Property.Create","Permission.Property.View",
            };

            var permissions = await context.Permissions.ToListAsync();
            var roles = await context.Roles.ToListAsync();

            foreach (var r in roles)
            {
                if (r.Name == Roles.Basic.ToString())
                {
                    var basicPerms = permissions.Where(p => basicPermission.Contains(p.Value!)).ToList();

                    await roleManager.SeedClaimsForRole(r, basicPerms, context);

                }
                if (r.Name == Roles.Owner.ToString() || r.Name == Roles.Admin.ToString())
                {
                    await roleManager.SeedClaimsForRole(r, permissions, context);
                }
                
            }
        }


        private async static Task SeedClaimsForRole(this RoleManager<Role> roleManager, Role role, List<Permission> permissions, AppDbContext context)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermission = context.Permissions.ToList();

            foreach (var permission in allPermission)
            {
                if (!allClaims.Any(a => a.Type == PermissionConstants.Permission && permissions.Any(p => p.Value == a.Value!)))
                {
                    await roleManager.AddClaimAsync(role, new Claim(PermissionConstants.Permission, permission.Value!));
                }
            }
        }
    }

}
