namespace _750HrsTracker.DTOs.Responses
{
    public class GetActivityLogResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public DateTime ActivityDate { get; set; }
        public int HoursSpent { get; set; }
        public int MinutesSpent { get; set; }
        public int SecondsSpent { get; set; }
        public string? Description { get; set; }

        public GetUserResponse? ActivityBy {  get; set; }
        public List<GetPropertyResponse>? Properties { get; set; }   
    }
}
