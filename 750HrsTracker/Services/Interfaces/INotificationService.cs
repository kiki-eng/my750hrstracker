using _750HrsTracker.DTOs.Requests;

namespace _750HrsTracker.Services.Interfaces
{
    public interface INotificationService
    {
        Task<bool> SendLoginNotification(LoginNotificationRequest request, bool isMobileRequest = false);
        Task<bool> SendInvitationNotification(InvitationNotificationRequest request, bool isMobileRequest = false, string appName = "750HrsTracker");
        Task<bool> SendSupportNotificationAsync(SupportRequest request, bool isMobileRequest = false, string appName = "750HrsTracker");
        Task<bool> SendEmailVerificationNotification(EmailVerificationNotificationRequest request, bool isMobileRequest = false);
        Task<bool> SendPasswordResetNotification(PasswordResetNotificationRequest request, bool isMobileRequest = false);
        Task<bool> NewAccountNotificationToAdmin(NewUserNotificationRequest request, bool isMobileRequest = false);
    }
}
