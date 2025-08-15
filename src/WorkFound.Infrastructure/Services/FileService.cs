using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WorkFound.Application.Common.Settings;
using WorkFound.Application.Files;

namespace WorkFound.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly Cloudinary _cloudinary;

    public FileService(IOptions<CloudinarySettings> config)
    {
        var cloudinarySettings = config.Value;
        
        if (string.IsNullOrEmpty(cloudinarySettings.CloudName) ||
            string.IsNullOrEmpty(cloudinarySettings.ApiKey) ||
            string.IsNullOrEmpty(cloudinarySettings.ApiSecret))
        {
            throw new ArgumentException("Cloudinary settings are not properly configured.");
        }
        
        var account = new Account(
            cloudinarySettings.CloudName,
            cloudinarySettings.ApiKey,
            cloudinarySettings.ApiSecret
        );
        
        _cloudinary = new Cloudinary(account);
    }
    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder, bool isImage)
    {
        UploadResult uploadResult;
        string safeFileName = GenerateSafeFileName(fileName, folder);
        if (isImage)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(safeFileName, fileStream),
                Folder = folder,
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }
        else
        {
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(safeFileName, fileStream),
                Folder = folder,
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }
        
        if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            throw new Exception($"Cloudinary upload failed: {uploadResult.Error?.Message}");

        return uploadResult.SecureUrl.AbsoluteUri;
    }

    public async Task<bool> DeleteFileAsync(string link)
    {
        var publicId = GetPublicIdFromUrl(link);

        if (string.IsNullOrEmpty(publicId)) return false;
        
        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);

        if (result.StatusCode != System.Net.HttpStatusCode.OK)
        {
            //todo: log error   
            return false;
        }

        return true;
    }

    #region Utility Methods

    private string GenerateSafeFileName(string originalFileName, string folderName)
    {
        var extension = Path.GetExtension(originalFileName).ToLowerInvariant();

        return $"{folderName}_{Guid.NewGuid()}{extension}";
    }
    
    private string? GetPublicIdFromUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return null;

        try
        {
            // Example: https://res.cloudinary.com/demo/image/upload/v123456/folder/filename.png
            var uri = new Uri(url);
            var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

            // Remove "image/upload" or "video/upload" or "raw/upload"
            var uploadIndex = Array.FindIndex(segments, s => s.Equals("upload", StringComparison.OrdinalIgnoreCase));
            if (uploadIndex == -1 || uploadIndex + 2 >= segments.Length)
                return null;

            // Remove version (starts with 'v' + number)
            var withoutVersion = segments.Skip(uploadIndex + 1).ToList();
            if (withoutVersion[0].StartsWith("v") && long.TryParse(withoutVersion[0].Substring(1), out _))
            {
                withoutVersion.RemoveAt(0);
            }

            // Join remaining segments and remove extension
            var publicIdWithExt = string.Join("/", withoutVersion);
            var publicId = Path.Combine(Path.GetDirectoryName(publicIdWithExt) ?? string.Empty,
                    Path.GetFileNameWithoutExtension(publicIdWithExt))
                .Replace("\\", "/");

            return publicId;
        }
        catch
        {
            return null; // In case of malformed URL
        }
    }

    #endregion
}