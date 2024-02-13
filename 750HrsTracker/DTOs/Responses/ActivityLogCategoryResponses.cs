using _750HrsTracker.Enums;
using _750HrsTracker.Models;

namespace _750HrsTracker.DTOs.Responses
{
    public class GetActivityLogCategoryResponse : BaseEntity
    {
        public string? Name { get; set; }
        public AvailablePropertyType PropertyType { get; set; }
    }
}
