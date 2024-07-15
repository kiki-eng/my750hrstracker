using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Enums;
using _750HrsTracker.Helpers.Constants;
using _750HrsTracker.Models;
using _750HrsTracker.Models.ActivityLogModels;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

namespace _750HrsTracker.Helpers
{
    public class Storage
    {
        public static async Task<ActivityLogDocument?> PrepareAndUploadDocumentAsync(IFormFile file, AppSettings _appSettings, Guid teamId, DocumentFor documentFor, string activityLog = null!)
        {
            var fileType = file.ContentType;

            var extension = Path.GetExtension(file.FileName).ToLower();
            var name = file.FileName.Split('.')[0];
            var fileName = Utility.UcWords(name).Replace(" ", "_").ToString().ToLower() + extension;

            var fileUploadResponse = await UploadDocumentAsync(_appSettings, file, documentFor, fileName, teamId.ToString(), activityLog);

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
        public static async Task<bool> RemoveDocumentsAsync(AppSettings appSettings, DocumentFor documentFor, string fileName, string teamId = null!, string activityLogId = null!)
        {
            string directoryName = "";
            string currentEnvironment = appSettings.CurrentEnvironment!;

            directoryName = documentFor switch
            {
                DocumentFor.ActivityLog => "activity-logs/" + teamId + "/" + activityLogId,
                DocumentFor.Template => $"{LogCategoryConstants.TemplatesDirectory}/{fileName}",
                DocumentFor.ProfilePicture => $"profile-pics/{fileName}",
                _ => throw new ApplicationException("Invalid document type"),
            };

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

            await blob.DeleteIfExistsAsync();

            return true;

        }
        public static async Task<DocumentUploadResponse> UploadDocumentAsync(AppSettings appSettings, IFormFile file, DocumentFor documentFor, string fileName, string teamId = null!, string activityLogId = null!)
        {
            try
            {

                string directoryName = "";
                string currentEnvironment = appSettings.CurrentEnvironment!;

                directoryName = documentFor switch
                {
                    DocumentFor.ActivityLog => "activity-logs/" + teamId + "/" + activityLogId +"/"+ fileName,
                    DocumentFor.Template => $"{LogCategoryConstants.TemplatesDirectory}/{fileName}",
                    DocumentFor.ProfilePicture => $"profile-pics/{fileName}",
                    _ => throw new ApplicationException("Invalid document type"),
                };

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

        public static async Task<byte[]> DownloadDocumentAsStream(AppSettings appSettings, string directoryName)
        {
             
            BlobServiceClient serviceClient = new(appSettings.AzureStorageBlobConnectionString);

            var containers = serviceClient.GetBlobContainers().FirstOrDefault(c => c.Name == appSettings.AzureStorageBlobContainerName);

            BlobContainerClient containerClient = serviceClient.GetBlobContainerClient(appSettings.AzureStorageBlobContainerName);

            BlobClient blobClient = containerClient.GetBlobClient(directoryName);

            if (await blobClient.ExistsAsync())
            {
                var tempUrl = GetDocTempUrl(appSettings, directoryName);

                // Create a memory stream to hold the blob data
                using MemoryStream stream = new();
                await blobClient.DownloadToAsync(stream);
                stream.Position = 0; // Reset the stream position

                // Return the stream as a FileStreamResult
                return stream.ToArray();
            }
            else
            {
                throw new KeyNotFoundException("Blob not found");
            }

        }

        public static Uri GetDocTempUrl(AppSettings appSettings, string filename)
        {
            string connectionString = appSettings.AzureStorageBlobConnectionString!;
            string containerName = appSettings.AzureStorageBlobContainerName!;
            string blobName = filename;

            Uri blobUri = GetBlobUri(connectionString, containerName, blobName);
            string sasToken = GenerateSasToken(connectionString, containerName, blobName);

            // Create a temporary URL by combining the blob URI and SAS token
            Uri temporaryUrl = new(blobUri, sasToken);

            return temporaryUrl;
        }

        public static Uri GetBlobUri(string connectionString, string containerName, string blobName)
        {
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            return blobClient.Uri;
        }

        public static string GenerateSasToken(string connectionString, string containerName, string blobName)
        {
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerClient.Uri.ToString(), // Use the container URI
                BlobName = blobClient.Name,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(30), // Adjust the expiration time as needed
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read); // Set the permissions as needed

            var sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(containerClient.AccountName, "ZAv7rjRE2jJNrP1FVaRiPg1u9kMvs1GMbdPKd2LeWewLMNqJ2aVUug+zBCblL7CYs7WJktNantKsfWi2wtFcXg=="));

            return sasToken.ToString();
        }

        public static string GetFileContentType(string filePath)
        {
            string Extension = Path.GetExtension(filePath).ToLower();
            string ContentType = Extension switch
            {
                FileExtensionConstants.FILE_EXTENSION_PDF => "application/pdf",
                FileExtensionConstants.FILE_EXTENSION_TXT => "text/plain",
                FileExtensionConstants.FILE_EXTENSION_PNG => "image/png",
                FileExtensionConstants.FILE_EXTENSION_JPG => "image/jpeg",
                FileExtensionConstants.FILE_EXTENSION_JPEG => "image/jpeg",
                FileExtensionConstants.FILE_EXTENSION_XLS => "application/vnd.ms-excel",
                FileExtensionConstants.FILE_EXTENSION_XLSX => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileExtensionConstants.FILE_EXTENSION_CSV => "text/csv",
                FileExtensionConstants.FILE_EXTENSION_HTML => "text/html",
                FileExtensionConstants.FILE_EXTENSION_XML => "text/xml",
                FileExtensionConstants.FILE_EXTENSION_ZIP => "application/zip",
                _ => "application/octet-stream",
            };
            return ContentType;
        }


    }
}
