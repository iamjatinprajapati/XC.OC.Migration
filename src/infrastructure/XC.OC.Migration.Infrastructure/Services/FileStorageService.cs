using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using XC.OC.Migration.Core.Application.Abstractions.Infrastructure;

namespace XC.OC.Migration.Infrastructure.Services;

public class FileStorageService(IConfiguration configuration, ILogger<FileStorageService> logger, IAzureStorageService azureStorageService) : IFileStorageService
{
    public List<string> GetFilesAsync(string folderName, string path)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteFileAsync(string folderName, string path)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveFileAsync(string content, string filePath)
    {
        await azureStorageService.UploadAsync(content, filePath);
        return true;
    }

    public Task MoveFileAsync(string sourceFileName, string destinationFileName)
    {
        throw new NotImplementedException();
    }
}