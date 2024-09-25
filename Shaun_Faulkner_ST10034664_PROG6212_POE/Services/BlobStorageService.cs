using Azure.Storage.Blobs;

namespace Shaun_Faulkner_ST10034664_PROG6212_POE.Services
{
    public class BlobStorageService
    {
        private readonly string _blobConnectionString;
        private readonly string _blobContainerName;

        public BlobStorageService(string blobconnectionString, string blobContainerName)
        {
            _blobConnectionString = blobconnectionString;
            _blobContainerName = blobContainerName;
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            BlobContainerClient containerClient = new BlobContainerClient(_blobConnectionString, _blobContainerName);
            await containerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, overwrite: true);

            return blobClient.Uri.ToString();
        }
    }
}
