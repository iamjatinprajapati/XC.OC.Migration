namespace XC.OC.Migration.Core.Application.Abstractions.Infrastructure;

public interface IFileStorageService
{
    public List<string> GetFilesAsync(string folderName, string path);
    
    public Task<bool> DeleteFileAsync(string folderName, string path);

    public Task<bool> SaveFileAsync(string content, string filePath);
    
    Task MoveFileAsync(string sourceFileName, string destinationFileName);
}