namespace _750HrsTracker.DTOs.Responses
{
    public class GetSubscriptionResponse
    {
        public Guid Id {  get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public decimal Price { get; set; }
        public int GracePeriodMinutes { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }


    public class UpdateSubscriptionPermissionResponse
    {
        public Guid SubscriptionId { get; set; }
        public string? SubscriptionName { get; set; }
        public List<GetPermissionResponse>? Permissions { get; set; }   
    }
}
