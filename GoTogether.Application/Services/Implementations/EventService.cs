using GoTogether.Application.Abstractions;
using GoTogether.Application.DTOs.Events;
using GoTogether.Application.DTOs.Files;
using GoTogether.Application.DTOs.Interests;
using GoTogether.Application.Services.Interfaces;
using GoTogether.Domain.Entities;

namespace GoTogether.Application.Services.Implementations;

public class EventService(IEventRepository events, IImagePathService paths, IImageStorageService storage) : IEventService
{
    public async Task<IEnumerable<EventListItemResponse>> GetAllEventsAsync()
    {
        var evs = await events.GetAllEventsAsync();

        return evs.Select(e => MapToResponse(e));
    }

    private EventListItemResponse MapToResponse(Event e)
    {
        var eventImagePath = paths.GetEventImagePath(e.ImageFileName);

        return new(e.Id, e.Title, e.StartsAt, e.Location, e.Category, eventImagePath, e.EventInterests.Count);
    }

    public async Task<EventDetailsResponse?> GetEventByIdAsync(Guid id) => MapToDto(await events.GetEventByIdAsync(id));

    public async Task<EventDetailsResponse> CreateEventAsync(CreateEventRequest req)
    {
        var e = MapToDto(await events.CreateEventAsync(req.Title, req.Description, req.StartsAt, req.Location, req.Category))!;
        await events.SaveChangesAsync();
        return e;
    }

    public async Task<EventDetailsResponse?> UpdateEventAsync(Guid eventId, UpdateEventRequest req)
    {
        var e = MapToDto(await events.UpdateEventAsync(eventId, req.Title, req.Description, req.StartsAt, req.Location, req.Category));
        await events.SaveChangesAsync();
        return e;
    }

    public Task DeleteEventAsync(Guid eventId) => events.DeleteEventAsync(eventId);

    public async Task<IEnumerable<EventInterestResponse>> GetEventInterestsAsync(Guid eventId)
    {
        var eventInterests = await events.GetEventInterestsAsync(eventId);

        return eventInterests.Select(ei => MapToResponse(ei));
    }

    private EventInterestResponse MapToResponse(EventInterest ei)
    {
        var avatarPath = paths.GetAvatarPath(ei.User.AvatarFileName);

        return new EventInterestResponse(
            avatarPath,
            ei.User.Username,
            ei.User.DisplayName,
            ei.Message,
            ei.CreatedAt
        );
    }

    public async Task<bool> GetUserInterestAsync(Guid userId, Guid eventId) => await events.GetUserInterestAsync(userId, eventId);

    public async Task SignalInterestAsync(Guid userId, Guid eventId, SignalEventInterestRequest req)
    {
        await events.SignalInterestAsync(userId, eventId, req.Message);
        await events.SaveChangesAsync();
    }

    public async Task<int> RemoveInterestAsync(Guid userId, Guid eventId) => await events.RemoveInterestAsync(userId, eventId);

    public async Task<string?> SaveEventImageAsync(Guid eventId, FileRequest file)
    {
        var ev = await events.GetEventByIdAsync(eventId);
        if (ev is null)
            return null;

        var fileName = await storage.SaveEventImageAsync(eventId, file);
        ev.SetImage(fileName);
        
        await events.SaveChangesAsync();

        return fileName;
    }

    private EventDetailsResponse? MapToDto(Event? e)
    {
        if (e is null)
            return null;

        return new EventDetailsResponse(
            e.Id,
            e.Title,
            e.Description,
            e.StartsAt,
            e.Location,
            e.Category,
            paths.GetEventImagePath(e.ImageFileName),
            e.EventInterests.Count
        );
    }
}
