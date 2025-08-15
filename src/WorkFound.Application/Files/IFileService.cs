using Microsoft.AspNetCore.Http;

namespace WorkFound.Application.Files;

public interface IFileService
{
    Task<string> UploadFileAsync(Stream stream, string fileName ,string folder, bool isImage);
    Task<bool> DeleteFileAsync(string publicId);
}