using GoTogether.Application.Abstractions;
using GoTogether.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoTogether.Infrastructure.Persistence.Repositories;

public class UserRepository(GoTogetherDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetUserByIdAsync(Guid userId) => await dbContext.Users.FindAsync(userId);

    public async Task<User?> GetUserByUsernameAsync(string username) => await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task<User?> GetUserByEmailAsync(string email) => await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task AddUserAsync(User user) => await dbContext.Users.AddAsync(user);

    public async Task<IEnumerable<EventInterest>> GetInterestedEventsByUserAsync(Guid userId) => await dbContext.EventInterests.Include(ei => ei.User).Include(ei => ei.Event).Where(ei => ei.UserId == userId).ToListAsync();

    public async Task SaveChangesAsync() => await dbContext.SaveChangesAsync();
}
