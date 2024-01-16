using _750HrsTracker.Models.Misc;
using static System.Net.Mime.MediaTypeNames;

namespace _750HrsTracker.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendMail(string to, string subject, string html);
        Task<bool> SendCustomEmailNotification(EmailDataUtil emailDataUtil);
    }
}
