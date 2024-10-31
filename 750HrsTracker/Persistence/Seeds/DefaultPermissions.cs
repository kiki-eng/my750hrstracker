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
                
                //Property
                new Permission { Module = "Property", Name = "Create"},
                new Permission { Module = "Property", Name = "View"},
                new Permission { Module = "Property", Name = "Update"},
                new Permission { Module = "Property", Name = "Delete"},
                new Permission { Module = "Property", Name = "AssignToUsers"},
                
                
                //Activity Log
                new Permission { Module = "ActivityLog", Name = "Create"},
                new Permission { Module = "ActivityLog", Name = "View"},
                new Permission { Module = "ActivityLog", Name = "Update"},
                new Permission { Module = "ActivityLog", Name = "Delete"},
                new Permission { Module = "ActivityLog", Name = "Import"},
                new Permission { Module = "ActivityLog", Name = "ExportDocument"},
                new Permission { Module = "ActivityLog", Name = "DownloadReport"},

                // Dashboard
                new Permission { Module = "Dashboard", Name = "View"},



                // Team
                new Permission { Module = "Team", Name = "AddRole"},
                new Permission { Module = "Team", Name = "GetRole"},
                new Permission { Module = "Team", Name = "UpdateRole"},
                new Permission { Module = "Team", Name = "DeleteRole"},
                new Permission { Module = "Team", Name = "InviteUser"},
                new Permission { Module = "Team", Name = "GetUsers"},
                new Permission { Module = "Team", Name = "MakeSpouse"},
                new Permission { Module = "Team", Name = "ActivateDeactivate"},
                new Permission { Module = "Team", Name = "DeleteAccount"},



                new Permission { Module = "Subscription", Name = "Manage"},

            };


            PermissionRepository permissionsRepository = new PermissionRepository(context);

            foreach (var permission in permissions)
            {
                var exists = await context.Permissions.FirstOrDefaultAsync(p => p.Module!.ToLower() == permission.Module!.ToLower() && p.Name!.ToLower() == permission.Name!.ToLower());
                if (exists == null)
                {
                    permission.Value = PermissionConstants.Permission + "." + permission.Module + "." + permission.Name;
                    permission.Slug = $"{PermissionConstants.Permission}_{permission.Module!}_{permission.Name!}".ToLower();
                    permission.Type = PermissionConstants.Permission;
                    await permissionsRepository.AddAsync(permission);
                }
                else
                {
                    exists.Value = PermissionConstants.Permission + "." + permission.Module + "." + permission.Name;
                    exists.Slug = $"{PermissionConstants.Permission}_{permission.Module!}_{permission.Name!}".ToLower();
                    exists.Type = PermissionConstants.Permission;

                    context.Permissions.Update(exists);
                    await context.SaveChangesAsync();   
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
                "Permission.Property.Create",
                "Permission.Property.View", 
                "Permission.ActivityLog.Create",
                "Permission.ActivityLog.View",
                "Permission.ActivityLog.Update",
                "Permission.Dashboard.View",
                "Permission.ActivityLog.ExportDocument",
                "Permission.ActivityLog.DownloadReport"
            };
            var taxPreparerPermissions = new List<string>
            {
                "Permission.ActivityLog.View", "Permission.Dashboard.View", "Permission.ActivityLog.ExportDocument","Permission.ActivityLog.DownloadReport"
            };

            var permissions = await context.Permissions.ToListAsync();
            var roles = await context.Roles.ToListAsync();

            foreach (var r in roles)
            {
                if(r.Name == Roles.TaxPreparer.ToString())
                {
                    var taxPreparerPerms = permissions.Where(p => taxPreparerPermissions.Contains(p.Value!)).ToList();

                    await roleManager.SeedClaimsForRole(r, taxPreparerPerms, context);
                }

                if (r.Name == Roles.DesignatedRep.ToString() || r.Name == Roles.Partner.ToString())
                {
                    var adminPermissions = permissions.Where(p => p.Value! != $"{PermissionConstants.Permission}.Team.DeleteAccount").ToList();

                    await roleManager.SeedClaimsForRole(r, adminPermissions, context);
                }
                
            }
        }


        private async static Task SeedClaimsForRole(this RoleManager<Role> roleManager, Role role, List<Permission> permissions, AppDbContext context)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermission = context.Permissions.ToList();

            foreach (var permission in permissions)
            {
                if (!allClaims.Any(a => a.Type == PermissionConstants.Permission && permission.Value == a.Value!))
                {
                    await roleManager.AddClaimAsync(role, new Claim(PermissionConstants.Permission, permission.Value!));
                }
            }
        }
    }

}
