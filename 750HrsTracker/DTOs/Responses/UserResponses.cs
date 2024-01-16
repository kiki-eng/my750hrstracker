namespace _750HrsTracker.DTOs.Responses
{
    public class SignInResponse
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string? Token { get; set; }
        public DateTime TokenExpireAt { get; set; }
    }

    public class GetUserResponse
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? DefaulTeamId { get; set; }

        public bool SendLoginNotification { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
