using _750HrsTracker.Enums;

namespace _750HrsTracker.DTOs.Responses
{
    public class GetDashboardResponse
    {
        public decimal TotalRepsHours { get; set; }
        public AvailablePropertyType PropertyType { get; set; }
        public List<LogTypeCounts>? LogHours { get; set; }
        public List<GetActivityLogResponse>? RecentLogs { get; set; }
    }
    public class LogTypeCounts
    {
        public string? LogType { get; set; }
        public decimal? TotalHours { get; set; }

        public List<GetCategoryHoursCount>? Categories { get; set; }

    }
    public class GetCategoryHoursCount
    {
        public string? Name { get; set; }
        public decimal Hours { get; set; }
    }
    
}
