using Azure.Storage.Blobs;
namespace EventEase.Service;

public class ImageService
{
    private readonly BlobContainerClient _containerClient;
    public ImageService(IConfiguration config)
    {
        var connectionString = config["BlobStorage:ConnectionString"];
        var containerName = config["BlobStorage:ContainerName"];
        _containerClient = new BlobContainerClient(connectionString, containerName);
        _containerClient.CreateIfNotExists();
    }

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var blob = _containerClient.GetBlobClient(fileName);
        await using var stream = file.OpenReadStream();
        await blob.UploadAsync(stream, true);
        return blob.Uri.ToString();
    }
}
