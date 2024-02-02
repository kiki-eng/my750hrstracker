using _750HrsTracker.Enums;

namespace _750HrsTracker.DTOs.Requests
{
    public class AddUpdatePropertyRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Alias { get; set; }

        public AvailablePropertyType PropertyType { get; set; }

        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
    }

    public class AssignPropertyToUsersRequest
    {
        public List<Guid>? UserIds { get; set; }
    }

}
