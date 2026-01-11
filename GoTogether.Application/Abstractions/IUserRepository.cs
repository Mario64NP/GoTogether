using GoTogether.Domain.Entities;

namespace GoTogether.Application.Abstractions;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<User?> GetUserByUsernameAsync(string username);
    Task AddUserAsync(User user);
    Task SaveChangesAsync();
}
