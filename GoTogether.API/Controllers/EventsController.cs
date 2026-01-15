using GoTogether.API.Extensions;
using GoTogether.Application.DTOs.Events;
using GoTogether.Application.DTOs.Files;
using GoTogether.Application.DTOs.Interests;
using GoTogether.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace GoTogether.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController(IEventService eventService, IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<EventListItemResponse>> GetEvents() => await eventService.GetAllEventsAsync();

    [Authorize]
    [HttpGet("{eventId}")]
    public async Task<ActionResult<EventDetailsResponse?>> GetEventById(Guid eventId) => Ok(await eventService.GetEventByIdAsync(eventId));

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<EventDetailsResponse>> CreateEvent(CreateEventRequest req)
    {
        var e = await eventService.CreateEventAsync(req);

        return CreatedAtAction(nameof(GetEventById), new { eventId = e.Id }, e);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{eventId}")]
    public async Task<ActionResult<EventDetailsResponse>> UpdateEvent(Guid eventId, UpdateEventRequest req) => Ok(await eventService.UpdateEventAsync(eventId, req));

    [Authorize(Roles = "Admin")]
    [HttpDelete("{eventId}")]
    public async Task<ActionResult> DeleteEvent(Guid eventId)
    {
        await eventService.DeleteEventAsync(eventId);

        return NoContent();
    }

    [HttpGet("{eventId}/interests")]
    public async Task<ActionResult<IEnumerable<EventInterestResponse>>> GetEventInterests(Guid eventId)
    {
        var e = await eventService.GetEventByIdAsync(eventId);

        if (e is null)
            return NoContent();

        return Ok(await eventService.GetEventInterestsAsync(eventId));
    }

    [Authorize]
    [HttpGet("{eventId}/interest")]
    public async Task<ActionResult<bool>> GetInterest(Guid eventId)
    {
        Guid userId = GetCurrentUserId();

        if (await eventService.GetEventByIdAsync(eventId) is null)
            return NotFound("Event not found");

        if (await userService.GetUserByIdAsync(userId) is null)
            return BadRequest("Invalid user");

        return Ok(await eventService.GetUserInterestAsync(userId, eventId));
    }

    [Authorize]
    [HttpPost("{eventId}/interest")]
    [EnableRateLimiting("strict")]
    public async Task<ActionResult> SignalInterest(Guid eventId, SignalEventInterestRequest req)
    {
        Guid userId = GetCurrentUserId();

        if (await eventService.GetEventByIdAsync(eventId) is null)
            return NotFound("Event not found");

        if (await userService.GetUserByIdAsync(userId) is null)
            return BadRequest("Invalid user");

        if (await eventService.GetUserInterestAsync(userId, eventId))
            return Conflict("User has already signaled interest for this event");

        await eventService.SignalInterestAsync(userId, eventId, req);

        return NoContent();

    }

    [Authorize]
    [HttpDelete("{eventId}/interest")]
    public async Task<ActionResult> RemoveInterest(Guid eventId)
    {
        Guid userId = GetCurrentUserId();

        if (!await eventService.GetUserInterestAsync(userId, eventId))
            return NotFound("Interest not found");

        await eventService.RemoveInterestAsync(userId, eventId);

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{eventId}/image")]
    [EnableRateLimiting("strict")]
    public async Task<ActionResult> UploadEventImage(Guid eventId, IFormFile file)
    {
        if (!await file.IsValidImageAsync())
            return BadRequest("Invalid image. Please upload a JPEG, PNG, or WebP under 5MB.");

        if (await eventService.GetEventByIdAsync(eventId) is null)
            return NotFound("Event not found.");

        using var stream = file.OpenReadStream();

        var req = new FileRequest(file.FileName, file.ContentType, stream);

        var fileName = await eventService.SaveEventImageAsync(eventId, req);

        return Ok(new { fileName });
    }

    private Guid GetCurrentUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
