using GoTogether.Application.DTOs.Interests;
using GoTogether.Domain.Entities;

namespace GoTogether.Application.Abstractions;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> UpdateUserAsync(Guid userId, string? displayname, string? bio, IEnumerable<string>? tags);
    Task<IEnumerable<EventInterest>> GetInterestedEventsByUserAsync(Guid userId);
    Task AddUserAsync(User user);
    Task SaveChangesAsync();
}
