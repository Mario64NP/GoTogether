using GoTogether.Application.Abstractions;
using GoTogether.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoTogether.Infrastructure.Persistence.Repositories;

public class EventRepository(GoTogetherDbContext dbContext) : IEventRepository
{
    async Task<IEnumerable<Event>> IEventRepository.GetAllEventsAsync()
    {
        var events = await dbContext.Events.Include(e => e.EventInterests).ToListAsync();
        return events;
    }

    public async Task<Event?> GetEventByIdAsync(Guid eventId) => await dbContext.Events.FindAsync(eventId);

    public async Task<Event> CreateEventAsync(string title, string? description, DateTime startsAt, string location, string category)
    {
        Event e = new(title, startsAt, location, category, description);

        await dbContext.Events.AddAsync(e);

        return e;
    }

    public async Task<Event?> UpdateEventAsync(Guid eventId, string? title, string? description, DateTime? startsAt, string? location, string? category)
    {
        var ev = await dbContext.Events.FindAsync(eventId);
        
        if (ev is null)
            return null;

        ev.Update(title, description, startsAt, location, category);

        return ev;
    }

    public async Task DeleteEventAsync(Guid eventId)
    {
        await dbContext.Events.Where(e => e.Id == eventId).ExecuteDeleteAsync(); 
        return;
    }

    public async Task<IEnumerable<EventInterest>> GetEventInterestsAsync(Guid eventId) => await dbContext.EventInterests.Include(ei => ei.User).Include(ei => ei.Event).Where(ei => ei.EventId == eventId).ToListAsync();

    public async Task<bool> GetUserInterestAsync(Guid userId, Guid eventId) => await dbContext.EventInterests.AnyAsync(ei => ei.User.Id == userId && ei.Event.Id == eventId);

    public async Task SignalInterestAsync(Guid userId, Guid eventId, string? message)
    {
        var ei = new EventInterest(userId, eventId, message);
        await dbContext.EventInterests.AddAsync(ei);
        return;
    }

    public async Task<int> RemoveInterestAsync(Guid userId, Guid eventId)
    {
        if (userId == Guid.Empty)
            return await dbContext.EventInterests.Where(ei => ei.EventId == eventId).ExecuteDeleteAsync();
        else
            return await dbContext.EventInterests.Where(ei => ei.UserId == userId && ei.EventId == eventId).ExecuteDeleteAsync();
    }

    public async Task SaveChangesAsync() => await dbContext.SaveChangesAsync();
}
