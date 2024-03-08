using _750HrsTracker.Enums;

namespace _750HrsTracker.DTOs.Responses
{
    public class DocumentUploadResponse
    {
        public string? FileAbsoluteUri { get; set; }
        public string? DirectoryName { get; set; }

    }

    public class DocumentUploadRequest
    {
        public IFormFile? File { get; set; }
        public string? FileName { get; set; }
        public DocumentFor DocumentFor { get; set; }
    }
}
