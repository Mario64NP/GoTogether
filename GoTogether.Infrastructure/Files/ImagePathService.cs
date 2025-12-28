using Microsoft.AspNetCore.Hosting;

namespace GoTogether.Infrastructure.Files;

public class ImagePathService(IWebHostEnvironment env) : IImagePathService
{
    private readonly IWebHostEnvironment _environment = env;

    public string GetAvatarDirectory()
    {
        return Path.Combine(_environment.ContentRootPath, "uploads", "images", "users");
    }

    public string GetEventImageDirectory()
    {
        return Path.Combine(_environment.ContentRootPath, "uploads", "images", "events");
    }

    public string? GetAvatarImagePath(string? imageFileName)
    {
        if (string.IsNullOrEmpty(imageFileName))
            return null;

        return $"uploads/images/users/{imageFileName}";
    }

    public string GetAvatarLocalImagePath(string imageFileName)
    {
        return Path.Combine(_environment.ContentRootPath, "uploads", "images", "users", imageFileName);
    }

    public string? GetEventImagePath(string? imageFileName)
    {
        if (string.IsNullOrEmpty(imageFileName))
            return null;

        return $"uploads/images/events/{imageFileName}";
    }

    public string GetEventLocalImagePath(string imageFileName)
    {
        return Path.Combine(_environment.ContentRootPath, "uploads", "images", "events", imageFileName);
    }
}
