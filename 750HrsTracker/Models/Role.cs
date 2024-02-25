using _750HrsTracker.Enums;
using _750HrsTracker.Models.JointEntities;
using Microsoft.AspNetCore.Identity;

namespace _750HrsTracker.Models
{
    public class Role : IdentityRole<Guid>
    {
        public string? Slug { get; set; }
        public Guid? TeamId { get; set; }
        public RoleType? RoleType { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatorId { get; set; }
        public bool Default { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedById { get; set; }
        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
