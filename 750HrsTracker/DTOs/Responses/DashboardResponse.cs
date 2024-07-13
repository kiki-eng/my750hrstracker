using _750HrsTracker.Enums;

namespace _750HrsTracker.DTOs.Responses
{
    public class GetDashboardResponse
    {
        public decimal TotalRepsHours { get; set; }
        public AvailablePropertyType PropertyType { get; set; }
        public List<LogTypeCounts>? LogHours { get; set; }
        public List<GetActivityLogResponse>? RecentLogs { get; set; }
        public List<UserHoursModel>? UserHours { get; set; }
        public int TeamMembersCount { get; set; }

    }
    public class LogTypeCounts
    {
        public string? LogType { get; set; }
        public decimal? TotalHours { get; set; }

        public List<GetCategoryHoursCount>? Categories { get; set; }

    }
    public class GetCategoryHoursCount
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public decimal Hours { get; set; }
        public List<UserHoursModel>? UserHours { get; set; } 
    }
    
    public class GetHoursResponse
    {
        public decimal? TotalStrHours { get; set; }
        public List<UserHoursModel>? UserHours { get; set; }
    }

    public class UserHoursModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal Hours { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSpouse { get; set; }
    }
}
