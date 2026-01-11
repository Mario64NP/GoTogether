using GoTogether.Application.DTOs.Events;
using GoTogether.Application.DTOs.Files;
using GoTogether.Application.DTOs.Interests;

namespace GoTogether.Application.Services.Interfaces;

public interface IEventService
{
    Task<IEnumerable<EventListItemResponse>> GetAllEventsAsync();
    Task<EventDetailsResponse?> GetEventByIdAsync(Guid eventId);
    Task<EventDetailsResponse> CreateEventAsync(CreateEventRequest req);
    Task<EventDetailsResponse?> UpdateEventAsync(Guid eventId, UpdateEventRequest req);
    Task DeleteEventAsync(Guid eventId);
    Task<IEnumerable<EventInterestResponse>> GetEventInterestsAsync(Guid eventId);
    Task<bool> GetUserInterestAsync(Guid userId, Guid eventId);
    Task SignalInterestAsync(Guid userId, Guid eventId, SignalEventInterestRequest req);
    Task<int> RemoveInterestAsync(Guid userId, Guid eventId);
    Task<string?> SaveEventImageAsync(Guid eventId, FileRequest file);
}
