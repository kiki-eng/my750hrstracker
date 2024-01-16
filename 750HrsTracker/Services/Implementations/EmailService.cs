using _750HrsTracker.Helpers;
using _750HrsTracker.Models.Misc;
using _750HrsTracker.Services.Interfaces;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;

namespace _750HrsTracker.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly AppSettings _appSettings;
        private readonly ISendGridClient _sendGridClient;
        public EmailService(IOptionsSnapshot<AppSettings> appSettings, ISendGridClient sendGridClient)
        {
            _appSettings = appSettings.Value;
            _sendGridClient = sendGridClient;

        }
        public async Task<bool> SendMail(string to, string subject, string html)
        {
            try
            {

                string emailFrom = _appSettings.SenderName!;
                string emailFromAddress = _appSettings.SenderAddress!;
                // create message
                var email = new MimeMessage();
                MailboxAddress fromAddress = new MailboxAddress(emailFrom, emailFromAddress);

                email.From.Add(fromAddress);
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = html };

                // send email   
                var message = new SendGridMessage()
                {
                    From = new EmailAddress(emailFromAddress, emailFrom),
                    Subject = subject,
                    HtmlContent = html
                };

                message.AddTo(new EmailAddress(to));

                var response = await _sendGridClient.SendEmailAsync(message);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                string exMsg = ex.Message;
                return false;
            }
        }

        public async Task<bool> SendCustomEmailNotification(EmailDataUtil emailDataUtil)
        {
            var message = $@"
                            <p>Hi {emailDataUtil.ReceipientName},</p>
                            <div>{emailDataUtil.MessageHtml}</div>
                            <br/>
                            ";
            try
            {
                var sent = await SendMail(emailDataUtil.ReceipientEmail!, emailDataUtil.Subject!, message);
                return sent;
            }
            catch
            {
                return false;
            }

        }
    }
}
