using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Enums;
using _750HrsTracker.Models;
using _750HrsTracker.Models.ActivityLogModels;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace _750HrsTracker.Helpers
{
    public class Storage
    {
        public static async Task<ActivityLogDocument?> PrepareAndUploadDocumentAsync(IFormFile file, AppSettings _appSettings, Guid teamId, DocumentFor documentFor)
        {
            var fileType = file.ContentType;

            var extension = Path.GetExtension(file.FileName).ToLower(); 

            var fileName = Utility.UcWords(documentFor.ToString()).Replace(" ", "_").ToString().ToLower() + extension;

            var fileUploadResponse = await UploadDocumentAsync(_appSettings, file, teamId.ToString(), documentFor, fileName);

            var filePath = fileUploadResponse.FileAbsoluteUri;

            ActivityLogDocument document = new ()
            {
                DocumentName = fileName,
                DocumentExtension = extension,
                DocumentType = fileType,
                DocumentPath = filePath,
                RemoteDirectoryName = fileUploadResponse.DirectoryName
            };

            return document;
        } 
        public static async Task<DocumentUploadResponse> UploadDocumentAsync(AppSettings appSettings, IFormFile file, string teamId, DocumentFor documentFor, string fileName)
        {
            try
            {

                string directoryName = "";
                string currentEnvironment = appSettings.CurrentEnvironment!;

                if (documentFor == DocumentFor.ActivityLog)
                {
                    directoryName = "activity-logs/" + teamId + "/" + fileName;
                }


                BlobServiceClient serviceClient = new(appSettings.AzureStorageBlobConnectionString);

                var containers = serviceClient.GetBlobContainers().FirstOrDefault(c => c.Name == appSettings.AzureStorageBlobContainerName);

                BlobContainerClient containerClient;

                if (containers == null)
                {
                    containerClient = serviceClient.CreateBlobContainer(appSettings.AzureStorageBlobContainerName, PublicAccessType.Blob);
                }
                else
                {
                    containerClient = new BlobContainerClient(appSettings.AzureStorageBlobConnectionString, appSettings.AzureStorageBlobContainerName);
                }

                BlobClient blob = containerClient.GetBlobClient(directoryName);

                using (Stream stream = file.OpenReadStream())
                {
                    await blob.UploadAsync(stream, true);
                }

                return new DocumentUploadResponse { FileAbsoluteUri = blob.Uri.AbsoluteUri, DirectoryName = directoryName };


            }
            catch 
            {
                throw;
            }

        }

    }
}
