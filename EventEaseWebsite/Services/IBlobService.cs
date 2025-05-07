using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using EventEase.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EventEase.Services
{
    public interface IBlobService
    {
        Task<string> UploadFileAsync(IFormFile file, string containerName);
        Task DeleteFileAsync(string blobName, string containerName); // <-- this line is required
    }
}
