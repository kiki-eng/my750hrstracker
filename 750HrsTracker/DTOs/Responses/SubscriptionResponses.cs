using _750HrsTracker.Enums;
using _750HrsTracker.Models;

namespace _750HrsTracker.DTOs.Responses
{
    public class GetSubscriptionResponse
    {
        public Guid Id {  get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public decimal Price { get; set; }

        public string? StripePriceId { get; set; }
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
}
