using _750HrsTracker.DTOs.Requests;

namespace _750HrsTracker.DTOs.Responses
{
    public class GetActivityLogResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public GetActivityLogCategoryResponse? Category { get; set; }
        public GetActivityLogActivityResponse? Activity { get; set; }
        public GetLogActivitySubCategoryResponse? Task { get; set; }
        public DateTime ActivityDate { get; set; }
        public int HoursSpent { get; set; }
        public int MinutesSpent { get; set; }
        public int SecondsSpent { get; set; }
        public string? Description { get; set; }

        public GetUsersOnlyResponse? ActivityBy {  get; set; }
        public List<GetPropertyResponse>? Properties { get; set; }   
        public List<Base64FileModel>? SupportingDocuments { get; set; }
    }
}
