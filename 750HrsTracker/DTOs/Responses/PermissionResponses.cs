namespace _750HrsTracker.DTOs.Responses
{
    public class GetPermissionResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Module {  get; set; }
        public string? Slug { get; set; }   
        public string? Value { get; set; }   
        public string? Type { get; set; }   
    }
}
