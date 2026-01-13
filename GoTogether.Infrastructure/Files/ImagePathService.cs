using GoTogether.Application.Services.Interfaces;
using GoTogether.Infrastructure.Persistence;

namespace GoTogether.Infrastructure.Files;

public class ImagePathService(InfrastructurePaths paths) : IImagePathService
{
    private readonly string rootPath = paths.Root;

    public string GetAvatarDirectory()
    {
        return Path.Combine(rootPath, "uploads", "images", "users");
    }

    public string GetEventImageDirectory()
    {
        return Path.Combine(rootPath, "uploads", "images", "events");
    }

    public string? GetAvatarPath(string? imageFileName)
    {
        if (string.IsNullOrEmpty(imageFileName))
            return null;

        return $"uploads/images/users/{imageFileName}";
    }

    public string GetAvatarLocalPath(string imageFileName)
    {
        return Path.Combine(GetAvatarDirectory(), Path.GetFileName(imageFileName));
    }

    public string? GetEventImagePath(string? imageFileName)
    {
        if (string.IsNullOrEmpty(imageFileName))
            return null;

        return $"uploads/images/events/{imageFileName}";
    }

    public string GetEventImageLocalPath(string imageFileName)
    {
        return Path.Combine(GetEventImageDirectory(), Path.GetFileName(imageFileName));
    }
}
