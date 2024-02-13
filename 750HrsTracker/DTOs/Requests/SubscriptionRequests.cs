namespace _750HrsTracker.DTOs.Requests
{
    public class AddUpdateSubscriptionRequest
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int GracePeriodMinutes { get; set; }
    }

   
}
