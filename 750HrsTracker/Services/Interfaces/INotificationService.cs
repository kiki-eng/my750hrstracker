using _750HrsTracker.DTOs.Requests;

namespace _750HrsTracker.Services.Interfaces
{
    public interface INotificationService
    {
        Task<bool> SendLoginNotification(LoginNotificationRequest request, bool isMobileRequest = false);
        Task<bool> SendInvitationNotification(InvitationNotificationRequest request, bool isMobileRequest = false);
        Task<bool> SendEmailVerificationNotification(EmailVerificationNotificationRequest request, bool isMobileRequest = false);
        Task<bool> SendPasswordResetNotification(PasswordResetNotificationRequest request, bool isMobileRequest = false);
        Task<bool> NewAccountNotificationToAdmin(NewUserNotificationRequest request, bool isMobileRequest = false);
    }
}
