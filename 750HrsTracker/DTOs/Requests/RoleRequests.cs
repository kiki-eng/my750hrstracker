namespace _750HrsTracker.DTOs.Requests
{
    public class AddRoleRequest
    {
        public string? RoleName { get; set; }
        public List<RolePermissionRequest>? Permissions { get; set; }
    }
    
    public class UpdateRoleRequest
    {
        public string? RoleName { get; set; }
    }

    public class RolePermissionRequest
    {
        public Guid Id { get; set; }
    }
    
    public class UpdateRolePermissionsRequest
    {
        public List<RolePermissionRequest>? Permissions { get; set; }

    }
}
