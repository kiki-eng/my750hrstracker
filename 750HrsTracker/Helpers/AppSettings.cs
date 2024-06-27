namespace _750HrsTracker.Helpers
{
    public class AppSettings
    {
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
        public string? StripeApiSecretKey { get; set; }
        public string? StripeProductId { get; set; }
        public string? StripeWebhookSecret { get; set; }
    }
}
