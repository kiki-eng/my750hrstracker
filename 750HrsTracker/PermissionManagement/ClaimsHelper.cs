using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Helpers.Constants;
using _750HrsTracker.Models;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using System.Security.Claims;

namespace _750HrsTracker.PermissionManagement
{
    public static class ClaimsHelper
    {

        public static void GetPermissions(this List<GetPermissionResponse> allPermissions, Type policy, string roleId)
        {
            FieldInfo[] fields = policy.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (FieldInfo fi in fields)
            {
                allPermissions.Add(new GetPermissionResponse { Value = fi.GetValue(null)!.ToString(), Type = PermissionConstants.Permission });
            }
        }

        public static async Task AddPermissionClaim(this RoleManager<Role> roleManager, Role role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a => a.Type == PermissionConstants.Permission && a.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim(PermissionConstants.Permission, permission));
            }
        }

    }
}
