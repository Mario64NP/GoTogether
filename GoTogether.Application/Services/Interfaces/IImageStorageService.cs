using GoTogether.Application.DTOs.Files;

namespace GoTogether.Application.Services.Interfaces;

public interface IImageStorageService
{
    Task<string> SaveProfileAvatarAsync(Guid id, FileRequest file);
    void DeleteProfileAvatarAsync(string fileName);
    Task<string> SaveEventImageAsync(Guid id, FileRequest file);
    void DeleteEventImageAsync(string fileName);
}
