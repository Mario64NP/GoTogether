namespace GoTogether.Infrastructure.Files;

public interface IImagePathService
{
    string GetAvatarDirectory();
    string GetEventImageDirectory();
    string GetEventLocalImagePath(string fileName);
    string? GetEventImagePath(string? fileName);

    string GetAvatarLocalImagePath(string fileName);
    string? GetAvatarImagePath(string? fileName);
}
