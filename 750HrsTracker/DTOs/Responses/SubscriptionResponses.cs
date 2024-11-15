using _750HrsTracker.Enums;
using _750HrsTracker.Models;
using Newtonsoft.Json;

namespace _750HrsTracker.DTOs.Responses
{
    public class GetSubscriptionResponse
    {
        public Guid Id {  get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public decimal Price { get; set; }

        public string? StripePriceId { get; set; }
        public string? IosProductId { get; set; }
        public string? AndroidProductId { get; set; }
        public SubscriptionInterval SubscriptionInterval { get; set; }

        public int GracePeriodMinutes { get; set; }
        public List<string>? Features { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }


    public class UpdateSubscriptionPermissionResponse
    {
        public Guid SubscriptionId { get; set; }
        public string? SubscriptionName { get; set; }
        public string? StripePriceId { get; set; }

        public List<GetPermissionResponse>? Permissions { get; set; }   
    }

    public class CreateStripeCheckoutSessionResponse
    {
        public string? SessionId { get; set; }
    }

    public class CreateStripeMobilePaymentIntentResponse
    {
        public string? EphemeralKey { get; set; }
        public string? PaymentIntent { get; set; }

        public string? StripeCustomerId { get; set; }
    }

    public class GetSubscriptionTransactionResponse
    {
        public Guid Id { get; set; }

        public GetTeamResponse? Team { get; set; }

        public GetSubscriptionResponse? Subcription { get; set; }

        public bool IsCheckoutTransaction { get; set; }

        public string? InitialStripeSessionId { get; set; }
        public string? StripeCustomerId { get; set; }
        public string? StripeSubscriptionId { get; set; }
        public string? StripeInvoiceId { get; set; }
        public string? StripeEventId { get; set; }
        public string? StripeEventName { get; set; }
        public string? EventDataObject { get; set; }

        public GetUsersOnlyResponse? LastActionBy { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class IosInApp
    {
        [JsonProperty("quantity")]
        public string? Quantity { get; set; }

        [JsonProperty("product_id")]
        public string? ProductId { get; set; }

        [JsonProperty("transaction_id")]
        public string? TransactionId { get; set; }

        [JsonProperty("original_transaction_id")]
        public string? OriginalTransactionId { get; set; }

        [JsonProperty("purchase_date")]
        public string? PurchaseDate { get; set; }

        [JsonProperty("purchase_date_ms")]
        public string? PurchaseDateMs { get; set; }

        [JsonProperty("purchase_date_pst")]
        public string? PurchaseDatePst { get; set; }

        [JsonProperty("original_purchase_date")]
        public string? OriginalPurchaseDate { get; set; }

        [JsonProperty("original_purchase_date_ms")]
        public string? OriginalPurchaseDateMs { get; set; }

        [JsonProperty("original_purchase_date_pst")]
        public string? OriginalPurchaseDatePst { get; set; }

        [JsonProperty("is_trial_period")]
        public string? IsTrialPeriod { get; set; }

        [JsonProperty("in_app_ownership_type")]
        public string? InAppOwnershipType { get; set; }
    }

    public class IosReceipt
    {
        [JsonProperty("receipt_type")]
        public string? ReceiptType { get; set; }

        [JsonProperty("adam_id")]
        public int? AdamId { get; set; }

        [JsonProperty("app_item_id")]
        public int? AppItemId { get; set; }

        [JsonProperty("bundle_id")]
        public string? BundleId { get; set; }

        [JsonProperty("application_version")]
        public string? ApplicationVersion { get; set; }

        [JsonProperty("download_id")]
        public int? DownloadId { get; set; }

        [JsonProperty("version_external_identifier")]
        public int? VersionExternalIdentifier { get; set; }

        [JsonProperty("receipt_creation_date")]
        public string? ReceiptCreationDate { get; set; }

        [JsonProperty("receipt_creation_date_ms")]
        public string? ReceiptCreationDateMs { get; set; }

        [JsonProperty("receipt_creation_date_pst")]
        public string? ReceiptCreationDatePst { get; set; }

        [JsonProperty("request_date")]
        public string? RequestDate { get; set; }

        [JsonProperty("request_date_ms")]
        public string? RequestDateMs { get; set; }

        [JsonProperty("request_date_pst")]
        public string? RequestDatePst { get; set; }

        [JsonProperty("original_purchase_date")]
        public string? OriginalPurchaseDate { get; set; }

        [JsonProperty("original_purchase_date_ms")]
        public string? OriginalPurchaseDateMs { get; set; }

        [JsonProperty("original_purchase_date_pst")]
        public string? OriginalPurchaseDatePst { get; set; }

        [JsonProperty("original_application_version")]
        public string? OriginalApplicationVersion { get; set; }

        [JsonProperty("in_app")]
        public List<IosInApp>? InApp { get; set; }
    }

    public class IosReceiptVerificationResponse
    {
        [JsonProperty("receipt")]
        public IosReceipt? Receipt { get; set; }
        
        [JsonProperty("environment")]
        public string? Environment { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }
    }


}
