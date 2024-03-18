using System.ComponentModel.DataAnnotations;

namespace _750HrsTracker.DTOs.Requests
{
    public class SignUpRequest
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set;}
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
    public class SignInRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class UpdateUserSecurityRequest
    {
        public bool SendLoginNotification { get; set; }
    }

    public class UpdateUserRequest 
    {
        public string? Firstname { get; set;}
        public string? Lastname { get; set;}
        
    }
    
    public class UpdateProfilePictureRequest 
    {
        public Base64FileModel? ProfilePicture { get; set; }
        
    }


    public class RecoverPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string? EmailAddress { get; set; }
    }

    public class ResetPasswordRequest
    {
        [Required]
        public string? ResetToken { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }

    public class ChangePasswordRequest
    {
        [Required]
        public string? OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmNewPassword { get; set; }
    }

    public class VerifyEmailRequest
    {
        [Required]
        public string? VerificationToken { get; set; }

    }

    public class ResendVerifyEmailRequest
    {
        [Required]
        public string? EmailAddress { get; set; }

    }

    public class InviteUserRequest
    {
        public string? EmailAddress { get; set; }
        public Guid RoleId { get; set; }
    }

    public class ValidateInvitationRequest
    {
        public string? InvitationCode { get; set; }
    }
    public class CreateInvitedUserRequest
    {
        public string? InvitationCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }

    public class MakeSpouseRequest
    {
        public Guid UserId { get; set; }
    }
    
}
