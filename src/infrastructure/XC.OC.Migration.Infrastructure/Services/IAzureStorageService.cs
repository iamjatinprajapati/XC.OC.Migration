using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace XC.OC.Migration.Infrastructure.Services;

public interface IAzureStorageService
{
    BlobClient GetBlobClient(string containerName, string blobPath);

    Task<GetAllBlobItem> ListBlobsAsync(string containerName = "", int pageSizeHing = 2000, string prefix = null,
        string blobName = null, string continuationToken = null, CancellationToken cancellationToken = default);

    Task<bool> UploadAsync(string content, string filePath, string containerName = "");

    Task MoveAsync(string containerName, string source, string destination);
}

public class GetAllBlobItem
{
    public IReadOnlyList<BlobItem> BlobItems { get; set; }

    public string ContinuationToken { get; set; }
}