namespace _750HrsTracker.Helpers
{
    public class AppSettings
    {
        public string? AdminAuthPolicy { get; set; }
        public string? UserAuthPolicy { get; set; }
        public string? JwtSecret { get; set; }
        public string? AppBaseUrl { get; set; }
        public string? SystemNotificationReceiverEmail { get; set; }
        public string? SystemNotificationReceiverName { get; set; }
        public string? NotificationOrigin { get; set; }
        public int VerificationTokenValidHours { get; set; }
        public int InvitationTokenExpiresMinutes { get; set; }
        public int JwtTokenTTLMinutees { get; set; }
        public int ResetTokenValidHours { get; set; }

        public string? SenderName { get; set; }
        public string? SenderAddress { get; set; }
        public string? FrontendBaseUrl { get; set; }

        public string? CurrentEnvironment { get; set;}
        public string? AzureStorageBlobConnectionString { get; set; }
        public string? AzureStorageBlobContainerName { get; set; }

        public string? PublicApiAccessKey { get; set; }
        public string? AdminApiAccessKey { get; set; }
        public string? StripeApiSecretKey { get; set; }
        public string? StripeProductId { get; set; }
        public string? StripeWebhookSecret { get; set; }
        public int StripeSubscriptionTrialPeriodDays { get; set; }
        public string? StripeApiVersion { get; set; }

        public string? AutoSuggestionBaseUrl { get; set; }
        public string? AutoSuggestionRequestUrl { get; set; }
        public string? AutoSuggestionApiKey { get; set; }

        public string? AdminFirstname { get; set; }
        public string? AdminLastname { get;set; }
        public string? AdminEmailAddress { get; set;}
    }
}
    