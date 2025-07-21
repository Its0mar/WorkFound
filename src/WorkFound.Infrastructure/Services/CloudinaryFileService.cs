using System.Net;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using WorkFound.Application.Common.Settings;
using WorkFound.Application.Files;

namespace WorkFound.Infrastructure.Services;

public class CloudinaryFileService : IFileService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryFileService(IOptions<CloudinarySettings> settings)
    {
        var acc = new Account(
            settings.Value.CloudName,
            settings.Value.ApiKey,
            settings.Value.ApiSecret
        );
        
        _cloudinary = new Cloudinary(acc);
    }
    public async Task<string?> UploadFileAsync(Stream fileStream, string fileName, string folder)
    {
        var uploadParams = new RawUploadParams
        {
            File = new FileDescription(fileName, fileStream),
            Folder = folder,
            UseFilename = true,
            UniqueFilename = true,
            Overwrite = false
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        return uploadResult.StatusCode == HttpStatusCode.OK
            ? uploadResult.SecureUrl.ToString()
            : null;
        
    }
}