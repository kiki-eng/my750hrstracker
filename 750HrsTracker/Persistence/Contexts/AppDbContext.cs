using _750HrsTracker.Models;
using _750HrsTracker.Models.JointEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _750HrsTracker.Persistence.Contexts
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid, UserClaims, UserRoles, UserLogins, RoleClaims, UserTokens>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {            
        }

        public new DbSet<User> Users { get; set; } 
        public DbSet<Team> Teams { get; set; } 
        public DbSet<TeamUser> Team_User { get; set; } 
        public new DbSet<Role> Roles { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<TeamUser>(entity =>
            {
                entity.HasKey(tu => new { tu.TeamId, tu.UserId });
                entity.HasOne(tu => tu.Team).WithMany(tu => tu.TeamUsers);
                entity.HasOne(tu => tu.User).WithMany(tu => tu.UserTeams);
            });

            builder.Entity<Role>(b =>
            {
                b.ToTable("Users");
            });

            builder.Entity<UserClaims>(b =>
            {
                b.ToTable("UserClaims");
            });
            builder.Entity<UserLogins>(b =>
            {
                b.ToTable("UserLogins");
            });
            builder.Entity<UserTokens>(b =>
            {
                b.ToTable("UserTokens");
            });
            builder.Entity<Role>(b =>
            {
                b.ToTable("Roles");
            });
            builder.Entity<RoleClaims>(b =>
            {
                b.ToTable("RoleClaims");
            });
            builder.Entity<UserRoles>(b =>
            {
                b.ToTable("UserRoles");
                b.HasKey(ur => new { ur.UserId, ur.RoleId, ur.TeamId });
            });
        }
    }
}
