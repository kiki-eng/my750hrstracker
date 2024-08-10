using _750HrsTracker.Models.JointEntities;
using _750HrsTracker.Models;

namespace _750HrsTracker.DTOs.Responses
{
    public class GetPropertyResponse
    {
        public Guid Id { get; set; }     

        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Alias { get; set; }
        public string? Code { get; set; }

        public string? Longitude { get; set; }
        public string? Latitude { get; set; }

        public Guid CreatedById { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }

    public class AdminGetPropertyResponse : GetPropertyResponse
    {
        public string? Type { get; set; }
        public string? Team { get; set; }
    }
}
