using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace XC.OC.Migration.Infrastructure.Services;

public class AzureStorageService : IAzureStorageService
{
    private readonly IConfiguration _configuration;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ILogger _logger;

    public AzureStorageService(IConfiguration configuration, ILogger<AzureStorageService> logger, BlobServiceClient client)
    {
        _logger = logger;
        _configuration = configuration;
        //var connectionString = _configuration["blobs"];
        _blobServiceClient = client;
    }

    public BlobClient GetBlobClient(string containerName, string blobPath)
    {
        BlobContainerClient? blobContainerClient = EnsureContainerExists(containerName);
        return blobContainerClient.GetBlobClient(blobPath);
    }

    public async Task<GetAllBlobItem> ListBlobsAsync(string containerName = "", int pageSizeHint = 2000, string prefix = null,
        string blobNames = null,
        string continuationToken = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(containerName))
        {
            containerName = _configuration["AzureStorage:ContainerName"];
        }

        BlobContainerClient? blobContainerClient = EnsureContainerExists(containerName);
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

    private BlobContainerClient EnsureContainerExists(string containerName)
    {
        BlobContainerClient? blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        blobContainerClient.CreateIfNotExists();
        return blobContainerClient;
    }

    public async Task<bool> UploadAsync(string content, string filePath, string containerName = "")
    {
        try
        {
            if(string.IsNullOrEmpty(containerName))
            {
                containerName = _configuration["AzureStorage:ContainerName"];
            }
            BlobContainerClient? blobContainerClient = EnsureContainerExists(containerName);
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
