using _750HrsTracker.Enums;

namespace _750HrsTracker.DTOs.Requests
{
    public class AddUpdateSubscriptionRequest
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int GracePeriodMinutes { get; set; }
        public SubscriptionInterval SubscriptionInterval { get; set; }
    }

    public class UpdateSubscriptionPriceRequest
    {
        public string? StripePriceId { get; set; }
    }

    public class UpdateSubscriptionFeaturesRequest
    {
        public List<string>? Features { get; set;}
    }
    
    public class UpdateSubscriptionPermissionRequest
    {
        public List<SubscriptionPermissionRequest>? Permissions { get; set;}
    }

    public class SubscriptionPermissionRequest
    {
        public Guid Id { get; set; }
    }

    public class CreateStripeCheckoutSessionRequest
    {
        public string? PriceId { get; set; }
    }
}
