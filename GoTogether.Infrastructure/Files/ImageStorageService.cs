using GoTogether.Application.DTOs.Files;
using GoTogether.Application.Services.Interfaces;

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

    public async Task<string> SaveProfileAvatarAsync(Guid id, FileRequest file)
    {
        var extension = Path.GetExtension(file.FileName);
        var localFileName = $"{id}{extension}";
        var localFilePath = _paths.GetAvatarLocalPath(localFileName);

        using var stream = new FileStream(localFilePath, FileMode.Create);
        await file.Content.CopyToAsync(stream);

        return localFileName;
    }

    public async void DeleteProfileAvatarAsync(string fileName)
    {
        var path = _paths.GetAvatarLocalPath(fileName);

        if (File.Exists(path))
            File.Delete(path);
    }

    public async Task<string> SaveEventImageAsync(Guid id, FileRequest file)
    {
        var extension = Path.GetExtension(file.FileName);
        var localFileName = $"{id}{extension}";
        var localFilePath = _paths.GetEventImageLocalPath(localFileName);

        using var stream = new FileStream(localFilePath, FileMode.Create);
        await file.Content.CopyToAsync(stream);

        return localFileName;
    }

    public async void DeleteEventImageAsync(string fileName)
    {
        var path = _paths.GetEventImageLocalPath(fileName);

        if (File.Exists(path))
            File.Delete(path);
    }
}
