using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.Enums;
using _750HrsTracker.Helpers;
using _750HrsTracker.Models.Misc;
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

        public async Task<bool> SendCustomNotificationAsync(NotificationDto notificationData, AppSettings appSettings)
        {
            string appName = "my750HrsTracker";
            string subject = $"{appName} Notification";
            string messageHtml = "";

            switch (notificationData.Event)
            {
                case NotificationEvent.subscription_trial_will_end:
                    subject = $"{appName} Subscription Trial Period Ending Soon";
                    messageHtml = $"<p>This is to inform you that your subscription to {appName} will end soon.</p>";
                    messageHtml += notificationData.Additional;
                    break;
                case NotificationEvent.subscription_payment_completed:
                    subject = $"{appName} Subscription Payment Successful";
                    messageHtml = $"<p>This is to inform you that your subscription to {appName} is successful.</p>";
                    messageHtml += notificationData.Additional;
                    break;
                
                case NotificationEvent.subscription_payment_failed:
                    subject = $"{appName} Subscription Failed";
                    messageHtml = $"<p>This is to inform you that your subscription to {appName} failed.</p>";
                    messageHtml += notificationData.Additional;
                    break;
                
                
                case NotificationEvent.subscription_checkout_session_completed:
                    subject = $"{appName} Subscription Checkout Session Completed";
                    messageHtml = $"<p>This is to inform you that your checkout session for subscription to {appName} was successful..</p>";
                    messageHtml += $"<p>Your subscription details will be shared shortly</p>";
                    messageHtml += notificationData.Additional;
                    break;

                default:
                    return false;
            }

            var html = $@"
                  <div id=""message-container"">
                    <p id=""salutation"">Dear {notificationData.RecipientName},</p>
                    <p class=""message-body msg"">
                      {messageHtml}
                    </p>
                    <p> 
                        Best Regards. 
                    </p>
                    <p> {appName} Team.</p>
                    <p> Copyright © {DateTime.Now.Year} {appName}. All rights reserved. </p> <p>201 Sand Creek Road, Suite F, Brentwood, CA 94513 </p>
                    <p>Phone: (925) 350-4963 | Fax: (925) 634-2346</p>
                </div>               
            ";

            try
            {
                var sent = await _emailService.SendMail(notificationData.RecipientEmail!, subject, html);
                return sent;
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> SendLoginNotification(LoginNotificationRequest request, bool isMobileRequest = false)
        {
            string appName = "my750HrsTracker";

            var html = $@"
                  <div id=""message-container"">
                    <p id=""salutation"">Dear {request.RecipientName},</p>
                    <p class=""message-body msg"">
                        We've received a sign-in attempt for your my750hrsTracker account. 
                        If this was you, please disregard this message. However, if you didn't attempt to sign in, please review your account security immediately.
                    </p>
                    <p>If you believe someone else may have accessed your account, we recommend taking the following steps:</p>
                    <ul>
                        <li>Change your password immediately.</li>
                        <li>Review your account settings for any unauthorized changes.</li>
                        <li>Contact our support team if you need further assistance or suspect any suspicious activity.</li>
                    </ul>

                    <p>Your security is our priority, and we're here to help ensure your account remains safe and secure.</p>
                    <p> 
                        Best Regards. 
                    </p>
                    <p> {appName} Team.</p>
                    <p> Copyright © {DateTime.Now.Year} {appName}. All rights reserved. </p> <p>201 Sand Creek Road, Suite F, Brentwood, CA 94513 </p>
                    <p>Phone: (925) 350-4963 | Fax: (925) 634-2346</p>
                </div>               
            ";

            try
            {
                var sent = await _emailService.SendMail(request.RecipientEmail!, "Action Required: Your my750hrsTracker Sign-In Attempt", html);
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

            string appName = "my750HrsTracker";
            var html = $@"
                <div>
                    <p>Dear {request.RecipientName},</p>
                    <p>
                        Welcome to {appName} – your ultimate Real Estate Professional tracking solution! We're thrilled to have you on board.
                    </p>
                    <p>
                        To ensure your account's security and activate your {appName} journey, we need to verify your email address. Please enter the 8-digit code provided below in your app:
                    </p>
                    <p>
                        <b>{request.VerifyEmailToken}</b>
                    </p>
                    <p>
                        We're dedicated to crafting the finest experience for Real Estate Professionals like you, and we can't wait to embark on this journey together. 
                        Expect regular updates, helpful tips, and exciting features tailored just for you.
                    </p>
                    <p> 
                        Should you have any questions or need assistance, don't hesitate to reach out. We're here to help.
                    </p>
                    <br/>
                    <p> 
                        Happy Tracking!
                    </p>
                    <p> 
                        Best Regards. 
                    </p>
                    <p> {appName} Team.</p>
                    <p> Copyright © {DateTime.Now.Year} {appName}. All rights reserved. </p> <p>201 Sand Creek Road, Suite F, Brentwood, CA 94513 </p>
                    <p>Phone: (925) 350-4963 | Fax: (925) 634-2346</p>

                   
                </div>
                <br/>
            ";

            try
            {
                var sent = await _emailService.SendMail(request.RecipientEmail!, "Welcome to my750hrsTracker - Verify Your Email Address!", html);
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

            string message = request.ExistingUser ? "Your login details remains the same after following through the process" : "";

            var html = $@"
                 <div id=""message-container"">
                    <p id=""salutation"">Hello there,</p>
                    <p class=""message-body"">
                        You have been invited by {request.InviterName} to join {request.TeamName} on {appName}.
                    </p>
                    <p class=""message-body"">
                      {(request.ExistingUser ? "Click the link below to accept invitation and login using your existing credentials" : "Copy to token below and follow the link to access application and accept the invitation")}.
                    </p>
                    <p class=""message-body"">
                       {message}
                    </p>
                </div>
                <br/>
                <div>
                    <b>{(request.ExistingUser ? "" : request.InvitationCode)}</b>
                </div>
                <div>
                   <p> Link => <a href=""{request.InvitationLink}"" target=""_blank"">Click to accept invitation</a> </p>
                </div>
            ";
            try
            {
                var sent = await _emailService.SendMail(request.RecipientEmail!, $"750HrsTracker: {(request.ExistingUser ? "" : "New ")}User Invitation!", html);
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

        public  async Task<bool> SendSupportNotificationAsync(SupportRequest request, bool isMobileRequest = false, string appName = "750HrsTracker")
        {
            try
            { 

                var emailData = new EmailDataUtil
                {
                    ReceipientEmail = _appSettings.SystemNotificationReceiverEmail,
                    ReceipientName = _appSettings.SystemNotificationReceiverName,
                    SenderEmail = request.Email,
                    SenderName = $"{request.FirstName} {request.LastName}",
                    Subject = $"[Action Required]: Support Request - {request.Subject}"
                };

                var html = $@"
                     <div>
                        <p>
                            {emailData.SenderName} ({emailData.SenderEmail} - {request.PhoneNumber}) has raised a support request with the following details below:
                        </p>
                        <p><b>Subject:</b> {request.Subject}</p>
                        <p><b>Message:</b> {request.Message}</p>
                    </div>
                    <br/>
                    <div>
                        <p>Best Regards.</p>
                        <p> {appName} Team.</p>
                        <p> Copyright © {DateTime.Now.Year} {appName}. All rights reserved. </p> 
                        <p>201 Sand Creek Road, Suite F, Brentwood, CA 94513 </p>
                        <p>Phone: (925) 350-4963 | Fax: (925) 634-2346</p>                   
                    </div>
                ";

                emailData.MessageHtml = html;

                return await _emailService.SendCustomEmailNotification(emailData);

            }
            catch
            {
                return false;
            }
        }
    }
}
