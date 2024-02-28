using _750HrsTracker.Models;
using _750HrsTracker.Models.ActivityLogModels;
using _750HrsTracker.Models.JointEntities;
using _750HrsTracker.Models.SubscriptionModels;
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
        public DbSet<AvailableProperty> Properties { get; set; }
        public DbSet<PropertyTeamUser> PropertyTeamUsers { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<ActivityLogDocument> ActivityLogDocuments { get; set; }
        public DbSet<ActivityLogProperty> ActivityLogProperties { get; set; }
        public DbSet<ActivityLogCategory> ActivityLogCategories { get; set; }
        public DbSet<ActivityLogActivity> ActivityLogActivities { get; set; }
        public DbSet<ActivityLogSubCategory> ActivityLogSubCategories { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<TeamSubscription> TeamSubscriptions { get; set; }
        public DbSet<SubscriptionPermission> SubscriptionPermissions { get; set; }
        public DbSet<UserInvitation> UserInvitations { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<TeamUser>(entity =>
            {
                entity.HasKey(tu => new { tu.TeamId, tu.UserId });
                entity.HasOne(tu => tu.Team).WithMany(tu => tu.TeamUsers).HasForeignKey(tu => tu.TeamId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(tu => tu.User).WithMany(tu => tu.UserTeams).HasForeignKey(tu => tu.UserId).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<AvailableProperty>(entity =>
            {
                entity.HasOne(ap => ap.CreatedBy).WithMany(ap => ap.PropertiesCreated).HasForeignKey(ap => ap.CreatedById);
                entity.HasOne(ap => ap.Team).WithMany(ap => ap.Properties).HasForeignKey(ap => ap.TeamId);

            });

            builder.Entity<PropertyTeamUser>(entity =>
            {
                entity.HasKey(tu => new { tu.TeamId, tu.UserId, tu.PropertyId });
                entity.HasOne(tu => tu.Team).WithMany(tu => tu.PropertyTeamUsers).HasForeignKey(tu => tu.TeamId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(tu => tu.User).WithMany(tu => tu.PropertyTeamUsers).HasForeignKey(tu => tu.UserId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(tu => tu.Property).WithMany(tu => tu.PropertyTeamUsers).HasForeignKey(tu => tu.PropertyId).OnDelete(DeleteBehavior.NoAction);
            });
            
            builder.Entity<ActivityLogProperty>(entity =>
            {
                entity.HasKey(tu => new { tu.ActivityLogId, tu.PropertyId });
                entity.HasOne(tu => tu.Property).WithMany(tu => tu.ActivityLogProperties).HasForeignKey(tu => tu.PropertyId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(tu => tu.ActivityLog).WithMany(tu => tu.ActivityLogProperties).HasForeignKey(tu => tu.ActivityLogId).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<ActivityLog>(entity =>
            {
                entity.HasOne(e => e.ActivityBy).WithMany(e => e.ActivityLogs).HasForeignKey(e => e.ActivityById).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.CreatedBy).WithMany(e => e.LogsCreated).HasForeignKey(e => e.CreatedById).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Team).WithMany(e => e.ActivityLogs).HasForeignKey(e => e.TeamId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.ActivityLogActivity).WithMany(e => e.ActivityLogs).HasForeignKey(e => e.ActivityLogActivityId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.ActivityLogCategory).WithMany(e => e.ActivityLogs).HasForeignKey(e => e.ActivityLogCategoryId).OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(e => e.ActivityLogDocuments).WithOne(e => e.ActivityLog).HasForeignKey(e => e.ActivityLogId).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<ActivityLogActivity>(entity =>
            {
                entity.HasMany(e => e.ActivityLogSubCategories).WithOne(e => e.LogActivity).HasForeignKey(e => e.LogActivityId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.ActivityLogCategory).WithMany(e => e.ActivityLogActivities).HasForeignKey(e => e.ActivityLogCategoryId).OnDelete(DeleteBehavior.NoAction);
                entity.HasIndex(e => e.Slug).IsUnique();
            });

            builder.Entity<Subscription>(entity =>
            {
                entity.HasIndex(e => e.Slug).IsUnique();
                entity.Property(e => e.Price).HasColumnType("Decimal").HasPrecision(18, 2);
            });

            builder.Entity<SubscriptionPermission>(entity =>
            {
                entity.HasKey(e => new { e.SubscriptionId, e.PermissionId });
                entity.HasOne(s => s.Subscription).WithMany(s => s.SubcriptionPermissions).HasForeignKey(s => s.SubscriptionId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(s => s.Permission).WithMany(s => s.SubscriptionPermissions).HasForeignKey(s => s.PermissionId).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<TeamSubscription>(entity =>
            {
                entity.HasKey(e => new { e.SubscriptionId, e.TeamId }); 
                entity.HasOne(e => e.Subscription).WithMany(e => e.TeamSubscriptions).HasForeignKey(e => e.SubscriptionId).OnDelete(DeleteBehavior.NoAction);   
                entity.HasOne(e => e.Team).WithMany(e => e.TeamSubscriptions).HasForeignKey(e => e.SubscriptionId).OnDelete(DeleteBehavior.NoAction);   
            });

            builder.Entity<ActivityLogCategory>(entity =>
            {
                entity.HasIndex(e => e.Slug).IsUnique();
            });
            
            builder.Entity<ActivityLogSubCategory>(entity =>
            {
                entity.HasIndex(e => e.Slug).IsUnique();
            });

            builder.Entity<User>(b =>
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
            
            builder.Entity<RolePermission>(b =>
            {
                b.HasKey(ur => new { ur.RoleId, ur.PermissionId });
                b.HasOne(rp => rp.Role).WithMany(rp => rp.RolePermissions).HasForeignKey(rp => rp.RoleId);
                b.HasOne(rp => rp.Permission).WithMany(rp => rp.RolePermissions).HasForeignKey(rp => rp.PermissionId);
            });

            builder.Entity<UserInvitation>(b =>
            {
                b.HasIndex(ui => ui.Code).IsUnique();
            });
        }
    }
}
