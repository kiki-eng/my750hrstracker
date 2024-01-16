namespace _750HrsTracker.Helpers
{
    public class AppSettings
    {
        public string? JwtSecret { get; set; }
        public string? AppBaseUrl { get; set; }
        public string? SystemNotificationReceiver { get; set; }
        public string? NotificationOrigin { get; set; }
        public int VerificationTokenValidHours { get; set; }    
        public int JwtTokenTTLMinutees { get; set; }    
        public int ResetTokenValidHours { get; set; }    

        public string? SenderName { get; set; }
        public string? SenderAddress { get; set; }
        public string? FrontendBaseUrl { get; set; }
    }
}
