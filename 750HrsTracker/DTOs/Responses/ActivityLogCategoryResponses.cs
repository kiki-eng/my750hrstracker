using _750HrsTracker.Enums;
using _750HrsTracker.Models;

namespace _750HrsTracker.DTOs.Responses
{
    public class GetActivityLogCategoryResponse : BaseEntity
    {
        public string? Name { get; set; }
        public AvailablePropertyType PropertyType { get; set; }
    }
    
    public class GetLogCategoryResponse : BaseEntity
    {
        public string? Name { get; set; }
        public AvailablePropertyType PropertyType { get; set; }
        public List<GetActivityLogActivityResponse>? Activities { get; set; }
    }
}
