namespace GoTogether.Application.Services.Interfaces;

public interface IImagePathService
{
    string GetEventImageDirectory();
    string GetAvatarDirectory();

    string GetEventImageLocalPath(string fileName);
    string? GetEventImagePath(string? fileName);

    string GetAvatarLocalPath(string fileName);
    string? GetAvatarPath(string? fileName);
}
