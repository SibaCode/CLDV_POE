using Azure.Storage.Blobs;
using EventEase.Services;
using Azure.Storage.Blobs.Specialized; // For PublicAccessType
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;  // Import this for BlobHttpHeaders
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Azure.Storage.Blobs.Specialized;
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
       var blobServiceClient = new BlobServiceClient(_connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();

        var blobName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var blobClient = containerClient.GetBlobClient(blobName);

        using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, overwrite: true);
        }

        // Generate SAS token
        if (blobClient.CanGenerateSasUri)
        {
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = blobName,
                Resource = "b", // 'b' = blob
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(6)
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
            return sasUri.ToString();
        }

        // Fallback if SAS cannot be generated
        return blobClient.Uri.ToString();
    
}

public async Task DeleteFileAsync(string blobName, string containerName)
{
    var blobClient = new BlobContainerClient(_connectionString, containerName);
    var blob = blobClient.GetBlobClient(blobName);
    await blob.DeleteIfExistsAsync();
}
    }
}
