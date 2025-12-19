using GoTogether.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoTogether.Infrastructure.Persistence
{
    public class GoTogetherDbContext(DbContextOptions<GoTogetherDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<EventInterest> EventInterests => Set<EventInterest>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GoTogetherDbContext).Assembly);
        }
    }
}
