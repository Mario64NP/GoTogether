using GoTogether.Application.Abstractions;
using GoTogether.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoTogether.Infrastructure.Persistence.Repositories;

public class UserRepository(GoTogetherDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetUserByIdAsync(Guid userId) => await dbContext.Users.FindAsync(userId);

    public async Task<User?> GetUserByUsernameAsync(string username) => await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task AddUserAsync(User user) => await dbContext.Users.AddAsync(user);

    public async Task SaveChangesAsync() => await dbContext.SaveChangesAsync();
}
