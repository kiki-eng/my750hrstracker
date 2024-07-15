namespace _750HrsTracker.DTOs.Responses
{
    public class GetAutoSuggestionResponse
    {
        public List<AutoSuggestedLocation>? predictions {  get; set; }
    }
    public class AutoSuggestedLocation
    {
        public string? description { get; set; }
        public string? place_id { get; set; }
    }
}
