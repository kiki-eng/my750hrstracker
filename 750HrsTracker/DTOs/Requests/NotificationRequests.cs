using _750HrsTracker.Enums;

namespace _750HrsTracker.DTOs.Requests
{
    public class NewUserNotificationRequest
    {
        public string? RecipientName { get; set; }
        public string? RecipientEmail { get; set; }
        public string? TeamName { get; set; }
        public string? TeamOwnerName { get; set; }
        public string? TeamOwnerEmail { get; set; }
        public string? Link { get; set; }
        public DateTime CreationDate { get; set; }
        public string? Origin { get; set; }
        public string? OriginIpAddress { get; set; }
    }

    public class LoginNotificationRequest
    {
        public string? RecipientName { get; set; }
        public string? RecipientEmail { get; set; }
        public string? LoginTime { get; set; }
        public string? LocationIp { get; set; }
        public string? DeviceInfo { get; set; }
        public string? Origin { get; set; }
        public string? OriginIpAddress { get; set; }
    }

    public class EmailVerificationNotificationRequest
    {
        public string? RecipientName { get; set; }
        public string? RecipientEmail { get; set; }
        public string? VerifyEmailToken { get; set; }
        public string? Origin { get; set; }
        public string? OriginIpAddress { get; set; }
    }

    public class PasswordResetNotificationRequest
    {
        public string? RecipientName { get; set; }
        public string? RecipientEmail { get; set; }
        public string? ResetPasswordToken { get; set; }
        public string? Origin { get; set; }
        public string? OriginIpAddress { get; set; }
    }
    

    public class InvitationNotificationRequest
    {
        public string? RecipientEmail { get; set; }
        public string? InvitationCode { get; set; }
        public string? InviterName { get; set; }
        public string? InviterEmail { get; set; }
        public string? InvitationLink { get; set; }
        public string? TeamName { get; set; }
        public bool ExistingUser { get; set; }
        public string? Origin { get; set; }
        public string? OriginIpAddress { get; set; }
    }

    public class NotificationDto
    {
        public NotificationEvent Event { get; set; }
        public string? RecipientName { get; set; }
        public string? RecipientEmail { get; set; }
        public string? Other { get; set; }
        public string? Additional { get; set; }
    }
}
