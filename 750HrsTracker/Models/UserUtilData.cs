namespace _750HrsTracker.Models
{
    public class UserUtilData
    {
        public Guid Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public string? VerificationToken { get; set; }
        public DateTime? VerificationTokenExpires { get; set; }
    }
}
