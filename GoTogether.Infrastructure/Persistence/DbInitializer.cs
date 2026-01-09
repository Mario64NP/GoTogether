using GoTogether.Domain.Entities;

namespace GoTogether.Infrastructure.Persistence;

public static class DbInitializer
{
    public static void Seed(GoTogetherDbContext context)
    {
        if (!context.Events.Any())
        {
            var events = new[]
            {
                new Event(
                    "Techno Night",
                    DateTime.Now.AddDays(7),
                    "Drugstore, Belgrade",
                    "Music",
                    "Late-night techno session"
                ),
                new Event(
                    "Indie Gig",
                    DateTime.Now.AddDays(10),
                    "KC Grad, Belgrade",
                    "Music",
                    "Small venue, good vibes"
                ),
            };

            context.Events.AddRange(events);
        }

        if (!context.Users.Any())
        {
            var users = new[]
            {
                new User(
                    "urio",
                    "Ma. Rio"
                ),
                new User(
                    "igy.jov",
                    "Manijak"
                ),
            };

            context.Users.AddRange(users);
        }


        context.SaveChanges();
    }
}
