namespace _750HrsTracker.Models
{
    public class UserProfilePicture : BaseEntity
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public string? DocumentPath { get; set; }
        public string? DocumentName { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentExtension { get; set; }

        public string? RemoteDirectoryName { get; set; }

    }
}
