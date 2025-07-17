using Microsoft.AspNetCore.Http;

namespace WorkFound.Application.Files;

public interface IFileService
{
    Task<FileUploadResult> UploadFileAsync(IFormFile file, string folder);
    Task<byte[]> GetFileAsync(string relativePath);
    bool DeleteFile(string relativePath);
}