namespace WorkFound.Application.Files;

public interface IFilesUploaded
{
    public Task<string> UploadProfilePicAsync(string filePath);
}