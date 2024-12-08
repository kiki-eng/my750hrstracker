using _750HrsTracker.Models;

namespace _750HrsTracker.DTOs.Responses
{
    public class GetTeamResponse : BaseEntity
    {
        public string? Name { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class AdminGetTeamResponse : GetTeamResponse
    {
        public int TotalNumberOfLogs { get; set; }
        public decimal? TotalHours { get; set; }
        public decimal? TotalLtrHours { get; set; }
        public decimal? TotalStrHours { get; set; }
        public List<GetUsersOnlyResponse>? Users { get; set; }
        public List<GetPropertyResponse>? Properties { get; set; }
    }

    public class GetTeamOnlyResponse
    {
        public Guid Id { get; set; }
        public  string? Name { get; set; }
    }
}
