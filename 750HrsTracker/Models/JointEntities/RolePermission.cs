namespace _750HrsTracker.Models.JointEntities
{
    public class RolePermission : BaseEntity
    {
        public Guid? RoleId { get; set; }
        public Role? Role { get; set; }
        public Guid? PermissionId { get; set; }
        public Permission? Permission { get; set; }  
    }
}
