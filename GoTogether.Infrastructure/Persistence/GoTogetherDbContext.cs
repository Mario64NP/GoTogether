using GoTogether.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoTogether.Infrastructure.Persistence
{
    public class GoTogetherDbContext : DbContext
    {
        public GoTogetherDbContext(DbContextOptions<GoTogetherDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<EventInterest> EventInterests => Set<EventInterest>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
