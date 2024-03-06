using _750HrsTracker.Enums;

namespace _750HrsTracker.DTOs.Requests
{
    public class AddUpdateActivityLogActivityRequest
    {
        public string? Name { get; set; }
        public AvailablePropertyType PropertyType { get; set; }
        public Guid? ActivityLogCategoryId { get; set; }

    }
    
    public class SeedLogActivityRequest
    {
        public string? Name { get; set; }
        public AvailablePropertyType PropertyType { get; set; }
        public string? LogCategorySlug { get; set; }
        public List<AddUpdateLogActivitySubCategoryRequest>? SubCategories { get; set; }

    }

    public class AddUpdateLogActivitySubCategoryRequest
    {
        public string? Name { get; set; }
    }


}
