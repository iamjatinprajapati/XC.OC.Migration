using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace XC.OC.Migration.Infrastructure.Services;

public interface IAzureStorageService
{
    BlobClient GetBlobClient(string containerName, string blobPath);

    Task<GetAllBlobItem> ListBlobsAsync(string containerName, int pageSizeHing = 2000, string prefix = null,
        string blobName = null, string continuationToken = null, CancellationToken cancellationToken = default);

    Task<bool> UploadAsync(string content, string filePath);

    Task MoveAsync(string containerName, string source, string destination);
}

public class AzureStorageService : IAzureStorageService
{
    private readonly IConfiguration _configuration;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ILogger _logger;

    public AzureStorageService(IConfiguration configuration, ILogger<AzureStorageService> logger)
    {
        _logger = logger;
        _configuration = configuration;
        _blobServiceClient = new BlobServiceClient(_configuration["AzureStorage:ConnectionString"]);
    }

    public BlobClient GetBlobClient(string containerName, string blobPath)
    {
        BlobContainerClient? blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        return blobContainerClient.GetBlobClient(blobPath);
    }

    public async Task<GetAllBlobItem> ListBlobsAsync(string containerName, int pageSizeHint = 2000, string prefix = null,
        string blobNames = null,
        string continuationToken = null, CancellationToken cancellationToken = default)
    {
        BlobContainerClient? blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        IAsyncEnumerable<Page<BlobItem>> page = blobContainerClient.GetBlobsAsync(cancellationToken: cancellationToken, 
            prefix: prefix).AsPages(continuationToken, pageSizeHint: pageSizeHint);
        IAsyncEnumerator<Page<BlobItem>> blobsList = page.GetAsyncEnumerator(cancellationToken);
        await blobsList.MoveNextAsync();

        GetAllBlobItem result = new GetAllBlobItem
        {
            BlobItems = blobsList.Current.Values,
            ContinuationToken = blobsList.Current.ContinuationToken
        };
        if (string.IsNullOrEmpty(blobNames)) return result;
        
        var blobsToInclude = blobNames.Split('|', StringSplitOptions.RemoveEmptyEntries);
        result.BlobItems = result.BlobItems.Where(b => blobsToInclude.Contains(b.Name)).ToList();

        return result;
    }

    public async Task<bool> UploadAsync(string content, string filePath)
    {
        try
        {
            BlobContainerClient? blobContainerClient = _blobServiceClient.GetBlobContainerClient(_configuration["AzureStorage:ContainerName"]);
            await blobContainerClient.UploadBlobAsync(filePath, BinaryData.FromString(content));
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("UploadAsync (ERROR) - {message} - {detail}", ex.Message, ex.StackTrace);
        }

        return false;
    }

    public async Task MoveAsync(string containerName, string source, string destination)
    {
        try
        {
            var sourceBlobClient = GetBlobClient(containerName, source);
            var destinationPath = sourceBlobClient.Name.Replace(source, destination);
            var destinationBlobClient = GetBlobClient(containerName, destinationPath);
            await destinationBlobClient.StartCopyFromUriAsync(sourceBlobClient.Uri);
            await sourceBlobClient.DeleteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("MoveAsync (ERROR) - {message} - {detail}", ex.Message, ex.StackTrace);
        }
    }
}

public class GetAllBlobItem
{
    public IReadOnlyList<BlobItem> BlobItems { get; set; }

    public string ContinuationToken { get; set; }
}