using Microsoft.AspNetCore.Http;

namespace GoTogether.Infrastructure.Files;

public interface IImageStorageService
{
    Task<string> SaveProfileAvatar(Guid id, IFormFile file);
    void DeleteProfileAvatar(string fileName);
    Task<string> SaveEventImage(Guid id, IFormFile file);
    void DeleteEventImage(string fileName);
}
