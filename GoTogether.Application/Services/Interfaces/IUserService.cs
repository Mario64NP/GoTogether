using GoTogether.Application.DTOs.Files;
using GoTogether.Application.DTOs.Users;
using GoTogether.Application.DTOs.Interests;

namespace GoTogether.Application.Services.Interfaces;

public interface IUserService
{
    Task<UserDetailsResponse?> GetUserByIdAsync(Guid userId);
    Task<UserDetailsResponse?> GetUserByUsernameAsync(string username);
    Task<IEnumerable<UserInterestResponse>> GetInterestedEventsByUserAsync(Guid userId);
    Task<bool> SetRoleAsync(string username, string role);
    Task<string?> SaveAvatarAsync(Guid userId, FileRequest req);
}
