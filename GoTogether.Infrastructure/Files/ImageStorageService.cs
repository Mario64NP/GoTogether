using Microsoft.AspNetCore.Http;

namespace GoTogether.Infrastructure.Files;

public class ImageStorageService : IImageStorageService
{
    private readonly IImagePathService _paths;

    public ImageStorageService(IImagePathService paths)
    {
        _paths = paths;

        Directory.CreateDirectory(_paths.GetAvatarDirectory());
        Directory.CreateDirectory(_paths.GetEventImageDirectory());
    }

    public async Task<string> SaveProfileAvatar(Guid id, IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        var localFileName = $"{id}{extension}";
        var localFilePath = _paths.GetAvatarLocalImagePath(localFileName);

        using var stream = new FileStream(localFilePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return localFileName;
    }

    public void DeleteProfileAvatar(string fileName)
    {
        var path = _paths.GetAvatarLocalImagePath(fileName);

        if (File.Exists(path))
            File.Delete(path);
    }

    public async Task<string> SaveEventImage(Guid id, IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        var localFileName = $"{id}{extension}";
        var localFilePath = _paths.GetEventLocalImagePath(localFileName);

        using var stream = new FileStream(localFilePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return localFileName;
    }

    public void DeleteEventImage(string fileName)
    {
        var path = _paths.GetEventLocalImagePath(fileName);

        if (File.Exists(path))
            File.Delete(path);
    }
}
