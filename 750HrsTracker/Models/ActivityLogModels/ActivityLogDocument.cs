namespace _750HrsTracker.Models.ActivityLogModels
{
    public class ActivityLogDocument : BaseEntity
    {
        public string? DocumentName { get; set; }
        public string? DocumentPath { get; set; }
        public string? DocumentType { get; set;}
        public string? DocumentExtension { get; set;}
        public string? RemoteDirectoryName { get; set; }

        public Guid? TeamId { get; set; }
        public Team? Team { get; set; }        

        public Guid ActivityLogId { get; set; }
        public ActivityLog? ActivityLog { get; set; }
    }
}
