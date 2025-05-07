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
    await containerClient.SetAccessPolicyAsync(PublicAccessType.None); // Keep private

    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
    var blobClient = containerClient.GetBlobClient(fileName);

    using (var stream = file.OpenReadStream())
    {
        await blobClient.UploadAsync(stream, true);
    }

    // Generate SAS
    var sasBuilder = new BlobSasBuilder
    {
        BlobContainerName = containerName,
        BlobName = fileName,
        Resource = "b",
        ExpiresOn = DateTimeOffset.UtcNow.AddHours(12)
    };
    sasBuilder.SetPermissions(BlobSasPermissions.Read);

    return blobClient.GenerateSasUri(sasBuilder).ToString();
}
public async Task DeleteFileAsync(string blobName, string containerName)
{
    var blobClient = new BlobContainerClient(_connectionString, containerName);
    var blob = blobClient.GetBlobClient(blobName);
    await blob.DeleteIfExistsAsync();
}
    }
}
