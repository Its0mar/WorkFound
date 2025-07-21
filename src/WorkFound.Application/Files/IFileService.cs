using Microsoft.AspNetCore.Http;

namespace WorkFound.Application.Files;

public interface IFileService
{
    Task<string?> UploadFileAsync(Stream fileStream, string fileName, string folder);
    // Task<byte[]> GetFileAsync(string relativePath);
    // bool DeleteFile(string relativePath);
}