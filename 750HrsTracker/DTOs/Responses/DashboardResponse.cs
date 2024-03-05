using _750HrsTracker.Enums;

namespace _750HrsTracker.DTOs.Responses
{
    public class GetDashboardResponse
    {
        public decimal TotalRepsHours { get; set; }
        public AvailablePropertyType PropertyType { get; set; }
        public List<GetActivityLogResponse>? RecentLogs { get; set; }
    }

    
}
