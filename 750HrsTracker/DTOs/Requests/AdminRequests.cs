using System.ComponentModel.DataAnnotations;

namespace _750HrsTracker.DTOs.Requests
{
    public class SupportRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Message { get; set; }
        public string? Subject { get; set; }
    }
}
