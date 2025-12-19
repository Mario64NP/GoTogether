using GoTogether.Domain.Entities;

namespace GoTogether.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static void Seed(GoTogetherDbContext context)
        {
            if (context.Events.Any())
                return;

            var events = new[]
            {
                new Event(
                    "Techno Night",
                    DateTime.Now.AddDays(7),
                    "Drugstore, Belgrade",
                    "Late-night techno session"
                ),
                new Event(
                    "Indie Gig",
                    DateTime.Now.AddDays(10),
                    "KC Grad, Belgrade",
                    "Small venue, good vibes"
                ),
            };

            context.Events.AddRange(events);
            context.SaveChanges();
        }
    }
}
