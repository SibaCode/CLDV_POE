using Azure.Storage.Blobs;
using EventEase.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EventEase.Services
{
    public class AzureBlobService : IBlobService
    {
        private readonly string _connectionString;

        public AzureBlobService(IConfiguration configuration)
        {
            _connectionString = configuration.GetSection("AzureBlobStorage")["ConnectionString"];
        }

        public async Task<string> UploadFileAsync(IFormFile file, string containerName)
        {
            var blobClient = new BlobContainerClient(_connectionString, containerName);
            await blobClient.CreateIfNotExistsAsync();

            var blobName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blob = blobClient.GetBlobClient(blobName);

            using (var stream = file.OpenReadStream())
            {
                await blob.UploadAsync(stream, true);
            }

            return blob.Uri.ToString();
        }
        public async Task DeleteFileAsync(string blobName, string containerName)
{
    var blobClient = new BlobContainerClient(_connectionString, containerName);
    var blob = blobClient.GetBlobClient(blobName);
    await blob.DeleteIfExistsAsync();
}
    }
}
