using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Enums;
using _750HrsTracker.Helpers;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace _750HrsTracker.Services.Implementations
{
    public class DocumentService : IDocumentService
    {
        private readonly AppSettings _appSettings;
        public DocumentService(IOptionsSnapshot<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<ResponseHandler<DocumentUploadResponse>> UploadDocumentAsync(DocumentUploadRequest request)
        {
            ResponseHandler<DocumentUploadResponse> response = new();

            var file = request.File!;
            if (file.Length < 1)
            {
                throw new ApplicationException("invalid file submitted");
            }

            var extension = Path.GetExtension(file.FileName).ToLower();

            var fileName = request.FileName + extension;

            var fileUploadResponse = await Storage.UploadDocumentAsync(_appSettings, file, request.DocumentFor, fileName);

            response.Success = true;
            response.Message = "Document uploaded successfully";
            response.Data = fileUploadResponse;

            return response;

          
        }
    }
}
