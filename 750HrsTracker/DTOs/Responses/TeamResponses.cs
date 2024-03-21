using _750HrsTracker.Models;

namespace _750HrsTracker.DTOs.Responses
{
    public class GetTeamResponse : BaseEntity
    {
        public string? Name { get; set; }
        public Guid? OwnerId { get; set; }
        public GetUserResponse? Owner { get; set; }
    }
}
