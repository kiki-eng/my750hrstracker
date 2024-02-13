using _750HrsTracker.Enums;

namespace _750HrsTracker.DTOs.Requests
{
    public class AddActivityLogCategoryRequest
    {
        public string? Name { get; set; }
        public AvailablePropertyType PropertyType { get; set; }
        public List<AddUpdateActivityLogActivityRequest>? LogActivities { get; set; }
    }


    
    public class UpdateActivityLogCategoryRequest
    {
        public string? Name { get; set; }
        public AvailablePropertyType PropertyType { get; set; }

    }
}
