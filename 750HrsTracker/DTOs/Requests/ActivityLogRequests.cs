using _750HrsTracker.Models.ActivityLogModels;
using _750HrsTracker.Models;
using _750HrsTracker.Enums;

namespace _750HrsTracker.DTOs.Requests
{
    public class AddActivityLogRequest
    {
        public string? Name { get; set; }
        public DateTime ActivityDate { get; set; }
        public int HoursSpent { get; set; }
        public int MinutesSpent { get; set; }
        public int SecondsSpent { get; set; }
        public string? Description { get; set; }
        public Guid ActivityById { get; set; }
        public Guid ActivityLogActivityId { get; set; }
        public Guid? ActivityLogCategoryId { get; set; }

        public AvailablePropertyType PropertyType { get; set; }
        public ActivityLogType LogType { get; set; }

        public List<Guid>? PropertiesIds { get; set; }
        public List<Base64FileModel>? SupportingDocuments { get; set; }
    }


    public class Base64FileModel
    {
        public string? FileName { get; set; }
        public string? FileExtension { get; set; }
        public string? ContentType { get; set; }
        public string? Data { get; set; }
    }
    public class UpdateActivityLogRequest
    {
        public string? Name { get; set; }
        public DateTime ActivityDate { get; set; }
        public int HoursSpent { get; set; }
        public int MinutesSpent { get; set; }
        public int SecondsSpent { get; set; }
        public string? Description { get; set; }
    }
}
