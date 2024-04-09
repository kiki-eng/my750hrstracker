namespace _750HrsTracker.DTOs.Responses
{
    public class GetRoleResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public bool Default { get; set; }
    }

    public class GetRolesOnlyResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }   
    }
}
