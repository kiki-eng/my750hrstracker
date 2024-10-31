using _750HrsTracker.Enums;
using _750HrsTracker.Models;
using _750HrsTracker.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace _750HrsTracker.Persistence.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = Roles.DesignatedRep.ToString(),
                    Slug = Roles.DesignatedRep.ToString().ToLower(),
                    CreatedBy = "system",
                    RoleType = RoleType.team,
                    Default = true
                },
                new Role()
                {
                    Name = Roles.Partner.ToString(),
                    Slug = Roles.Partner.ToString().ToLower(),
                    CreatedBy = "system",
                    RoleType = RoleType.team,
                    Default = true
                },
                new Role()
                {
                    Name = Roles.TaxPreparer.ToString(),
                    Slug = Roles.TaxPreparer.ToString().ToLower(),
                    CreatedBy = "system",
                    RoleType = RoleType.team,
                    Default = true
                }
            };

            var existingRoles = await context.Roles.ToListAsync();
            foreach (var kd in roles)
            {
                if (!existingRoles.Any(d => d.Name.Trim() == kd.Name.Trim()))
                {
                    await context.Roles.AddAsync(kd);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
