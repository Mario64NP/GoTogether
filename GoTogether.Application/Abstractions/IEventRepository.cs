using GoTogether.Domain.Entities;

namespace GoTogether.Application.Abstractions;

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetAllEventsAsync();
    Task<Event?> GetEventByIdAsync(Guid eventId);
    Task<Event> CreateEventAsync(string title, string description, DateTime startsAt, string location, string category);
    Task<Event?> UpdateEventAsync(Guid eventId, string? title, string? description, DateTime? startsAt, string? location, string? category);
    Task DeleteEventAsync(Guid eventId);
    Task<IEnumerable<EventInterest>> GetEventInterestsAsync(Guid eventId);
    Task<bool> GetUserInterestAsync(Guid userId, Guid eventId);
    Task SignalInterestAsync(Guid userId, Guid eventId, string? message);
    Task<int> RemoveInterestAsync(Guid userId, Guid eventId);
    Task SaveChangesAsync();
}
