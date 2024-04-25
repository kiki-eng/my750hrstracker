namespace _750HrsTracker.Models.Admin
{
    public class AdminProfilePicture : BaseEntity
    {
        public Guid? AdminId { get; set; }
        public Admin? AdminUser { get; set; }
        public string? DocumentPath { get; set; }
        public string? DocumentName { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentExtension { get; set; }

        public string? RemoteDirectoryName { get; set; }

    }
}
