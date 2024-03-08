using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Models.ResponseWrappers;

namespace _750HrsTracker.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<ResponseHandler<DocumentUploadResponse>> UploadDocumentAsync(DocumentUploadRequest request);
    }
}
