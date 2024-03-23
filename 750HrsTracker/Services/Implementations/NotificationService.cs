using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.Helpers;
using _750HrsTracker.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace _750HrsTracker.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailService;
        public NotificationService(IOptionsSnapshot<AppSettings> appSettings, IEmailService emailService)
        {
            _appSettings = appSettings.Value;
            _emailService = emailService;
        }

        public async Task<bool> SendLoginNotification(LoginNotificationRequest request, bool isMobileRequest = false)
        {

            string supportEmail = "mailto:support@750hrstracker.com";

            var html = $@"
                  <div id=""message-container"">
                    <p id=""salutation"">Hi {request.RecipientName},</p>
                    <p class=""message-body msg"">
                        This is to notify you of the successful login to your account.
                    </p>

                    <div id=""login-details"">
                        <p>
                            <span class=""title"">Login Time: </span>
                            <span class=""info"">{request.LoginTime}</span>
                        </p>
                        <p>
                            <span class=""title"">Location & IP: </span>
                            <span class=""info"">{request.LocationIp}</span>
                        </p>
                        <p>
                            <span class=""title"">Device Info: </span>
                            <span class=""info"">{request.DeviceInfo}</span>
                        </p>
                    </div>
                    <p class=""message-body msg"">If you did not attempt login with the device above, reach out to our <a href=""{supportEmail}"">support team</a> immediately to disable your account.</p>
                </div>
                <div id=""button-holder"">
                    <a href=""{supportEmail}"" target=""_blank"" class=""btn"" style=""color: #F5F4F9"">Contact Support</a>
                </div>
                <div id=""remark"">
                    <p>Regards,</p>
                    <p>750HrsTracker Team</p>
                </div>
            ";

            try
            {
                var sent = await _emailService.SendMail(request.RecipientEmail!, "750HrsTracker: Login Notification", html);
                return sent;
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> SendEmailVerificationNotification(EmailVerificationNotificationRequest request, bool isMobileRequest = false)
        {
            string verificationUrl = $"{_appSettings.FrontendBaseUrl}/verify-email/{request.VerifyEmailToken}";
           
            var html = $@"
                <div>
                    <p>Welcome {request.RecipientName},</p>
                    <br/>
                    <p>
                        You have created a new account with 750HrsTracker.
                    </p>
                    <br/>
                    <p>
                        Verify your email and gain full access to the platform by using the code below
                    </p>
                    <p>
                        <b>{request.VerifyEmailToken}</b>
                    </p>
                </div>
                <br/>
            ";

            try
            {
                var sent = await _emailService.SendMail(request.RecipientEmail!, "750HrsTracker: Email Verification", html);
                return sent;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> SendPasswordResetNotification(PasswordResetNotificationRequest request, bool isMobileRequest = false)
        {

            var html = $@"
                <div id=""message-container"">
                    <p>Hello {request.RecipientName},</p>
                    <br/>
                    <p>
                        Reset your password and regain access to your account by using the token below
                    </p>
                </div>
                <br/>
                <div>
                    <b>{request.ResetPasswordToken}</b>
                </div>
            ";

            try
            {
                var sent = await _emailService.SendMail(request.RecipientEmail!, "750HrsTracker: Reset Password", html);
                return sent;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> SendInvitationNotification(InvitationNotificationRequest request, bool isMobileRequest = false, string appName = "750HrsTracker")
        {

            var html = $@"
                 <div id=""message-container"">
                    <p id=""salutation"">Hello there,</p>
                    <p class=""message-body"">
                        You have been invited by {request.InviterName} to join {request.TeamName} on {appName}.
                    </p>
                    <p class=""message-body"">
                       Copy to token below and follow the link to access application and accept the invitation.
                    </p>
                </div>
                <br/>
                <div>
                    <b>{request.InvitationCode}</b>
                </div>
                <div>
                   <p> Link => <a href==""{request.InvitationLink}"" target=""_blank"">Visit/Dowload application></a> </p>
                </div>
            ";
            try
            {
                var sent = await _emailService.SendMail(request.RecipientEmail!, "750HrsTracker: New User Invitation!", html);
                return sent;
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> NewAccountNotificationToAdmin(NewUserNotificationRequest request, bool isMobileRequest = false)
        {

            var html = $@"
                 <div id=""message-container"">
                    <p id=""salutation"">Hello there,</p>
                    <p class=""message-body"">
                        {request.TeamName} has been registered by {request.TeamOwnerName} ({request.TeamOwnerEmail}) on {request.CreationDate}.
                    </p>
                    <p class=""message-body"">
                        Click the button below to view user account
                    </p>
                </div>

                <div class=""button-holder"">
                    <a class=""btn"" style=""color: #F5F4F9"" href=""{request.Link}"" target=""_blank"">View User Account</a>
                </div>
            ";

            try
            {
                var sent = await _emailService.SendMail(request.RecipientEmail!, "750HrsTracker: New User Onboarded!", html);
                return sent;
            }
            catch
            {
                return false;
            }
        }


       
        
    }
}
