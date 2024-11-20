using _750HrsTracker.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

    public class ValidateInAppPurchaseReceiptRequest
    {
        public string? ProductId { get; set; }
        public string? TransactionId { get; set; }  
        public long TransactionDate { get; set; }  
        public DeviceType DeviceType { get; set; }
        public string? TransactionReceipt { get; set; }

    }

    public class AppleStoreNotificationRequest
    {
        [JsonProperty("signedPayload")]
        public string? SignedPayload { get; set; }
    }

    public class AppleStoreNotificationV2
    {
        [JsonProperty("notificationType")]
        public AppleNotificationTypes NotificationType { get; set; }

        [JsonProperty("subType")]
        public AppleNotificationSubTypes SubType { get; set; }

        [JsonProperty("notificationUUID")]
        public string? NotificationUUID { get; set; }

        [JsonProperty("notificationVersion")]
        public string? NotificationVersion { get; set; }

        [JsonProperty("data")]
        public string? Data { get; set; }
    }

    public class AppleStoreNotificationData
    {
        [JsonProperty("appAppleId")]
        public string? AppAppleId { get; set; }

        [JsonProperty("bundleId")]
        public string? BundleId { get; set; }

        [JsonProperty("bundleVersion")]
        public string? BundleVersion { get; set; }
        
        [JsonProperty("environment")]
        public AppleNotificationDataEnvironmentName Environment { get; set; }
        
        [JsonProperty("signedRenewalInfo")]
        public string? SignedRenewalInfo { get; set; }

        [JsonProperty("signedTransactionInfo")]
        public string? SignedTransactionInfo { get; set; }

    }
}
