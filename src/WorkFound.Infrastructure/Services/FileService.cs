using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using WorkFound.Application.Files;

namespace WorkFound.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;
    private readonly string _root = "uploads";
    
    public FileService(IWebHostEnvironment env)
    {
        _env = env;
    }
    
    public async Task<FileUploadResult> UploadFileAsync(IFormFile file, string folder)
    {
        var uploadsFolder = Path.Combine(_env.WebRootPath, _root, folder);
        Directory.CreateDirectory(uploadsFolder);
        
        var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream);
        var relativePath = Path.Combine(_root, folder, uniqueFileName).Replace("\\", "/");

        return new FileUploadResult(relativePath, uniqueFileName);
    }

    public async Task<byte[]> GetFileAsync(string relativePath)
    {
        var filePath = Path.Combine(_env.WebRootPath, relativePath);
        return await File.ReadAllBytesAsync(filePath);
    }

    public bool DeleteFile(string relativePath)
    {
        var filePath = Path.Combine(_env.WebRootPath, relativePath);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            return true;
        }

        return false;
    }
}