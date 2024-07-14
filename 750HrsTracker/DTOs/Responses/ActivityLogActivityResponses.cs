using _750HrsTracker.Enums;
using _750HrsTracker.Models;

namespace _750HrsTracker.DTOs.Responses
{
    public class GetActivityLogActivityResponse : BaseEntity
    {
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public AvailablePropertyType PropertyType { get; set; }
        public string? Category { get; set; }

        public List<GetLogActivitySubCategoryResponse>? Tasks { get; set; }
    }

    public class GetLogActivitySubCategoryResponse : BaseEntity
    {
        public string? Name { get; set;}
        public string? Slug { get; set;}
        public GetActivityLogActivityResponse? LogActivity { get; set;}

    }

}
