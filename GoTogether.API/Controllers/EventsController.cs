using GoTogether.API.Contracts.Events;
using GoTogether.API.Contracts.Interests;
using GoTogether.Domain.Entities;
using GoTogether.Infrastructure.Files;
using GoTogether.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoTogether.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController(GoTogetherDbContext dbContext, IWebHostEnvironment env, IImagePathService paths, IImageStorageService storage) : ControllerBase
{
    private readonly GoTogetherDbContext _dbContext = dbContext;
    private readonly IImagePathService _imagePaths = paths;
    private readonly IImageStorageService _imageStorage = storage;

    [HttpGet]
    public async Task<IEnumerable<EventListItemDto>> GetEvents()
    {
        return await _dbContext.Events
            .OrderBy(e => e.StartsAt)
            .Select(e => new EventListItemDto(
                e.Id,
                e.Title,
                e.StartsAt,
                e.Location,
                e.Category,
                _imagePaths.GetEventImagePath(e.ImageFileName),
                e.EventInterests.Count
            ))
            .ToListAsync();
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<EventDetailsDto>> GetEventById(Guid id)
    {
        var ev = await _dbContext.Events
            .Where(e => e.Id == id)
            .Select(e => new EventDetailsDto(
                e.Id,
                e.Title,
                e.Description,
                e.StartsAt,
                e.Location,
                e.Category,
                _imagePaths.GetEventImagePath(e.ImageFileName),
                e.EventInterests.Count
            ))
            .FirstOrDefaultAsync();

        if (ev is null)
            return NotFound();

        return Ok(ev);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<EventDetailsDto>> CreateEvent(CreateEventRequest req)
    {
        Event e = new(req.Title, req.StartsAt, req.Location, req.Category, req.Description);

        _dbContext.Events.Add(e);
        await _dbContext.SaveChangesAsync();

        var evDto = new EventDetailsDto(
            e.Id,
            e.Title,
            e.Description,
            e.StartsAt,
            e.Location,
            e.Category,
            _imagePaths.GetEventImagePath(e.ImageFileName),
            e.EventInterests.Count
        );

        return CreatedAtAction(
            nameof(GetEventById),
            new { id = e.Id },
            evDto
        );
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<EventDetailsDto>> UpdateEvent(Guid id, UpdateEventRequest req)
    {
        var ev = await _dbContext.Events.FindAsync(id);

        if (ev is null)
            return NotFound();

        ev.Update(
            req.Title,
            req.Description,
            req.StartsAt,
            req.Location,
            req.Category
        );

        await _dbContext.SaveChangesAsync();

        var evDto = new EventDetailsDto(
            ev.Id,
            ev.Title,
            ev.Description,
            ev.StartsAt,
            ev.Location,
            ev.Category,
            ev.ImageFileName,
            ev.EventInterests.Count
        );

        return Ok(evDto);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEvent(Guid id)
    {
        var ev = await _dbContext.Events.FindAsync(id);

        if (ev is null)
            return NotFound();

        if (!string.IsNullOrEmpty(ev.ImageFileName))
            _imageStorage.DeleteEventImage(ev.ImageFileName);

        _dbContext.Events.Remove(ev);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("{id}/interests")]
    public async Task<ActionResult<IEnumerable<EventInterestResponse>>> GetEventInterests(Guid id)
    {
        if (!await _dbContext.Events.AnyAsync(e => e.Id == id))
            return NotFound("Event not found");

        var interests = await _dbContext.EventInterests
            .Where(ei => ei.EventId == id)
            .Select(ei => new EventInterestResponse(
                _imagePaths.GetAvatarImagePath(ei.User.AvatarFileName),
                ei.User.Username,
                ei.User.DisplayName,
                ei.Message,
                ei.CreatedAt
            ))
            .ToListAsync();

        return Ok(interests);
    }

    [Authorize]
    [HttpGet("{id}/interest")]
    public async Task<ActionResult<bool>> GetInterest(Guid id)
    {
        Guid userId = GetCurrentUserId();

        if (!await _dbContext.Events.AnyAsync(e => e.Id == id))
            return NotFound("Event not found");

        if (!await _dbContext.Users.AnyAsync(u => u.Id == userId))
            return BadRequest("Invalid user");

        return Ok(await _dbContext.EventInterests.AnyAsync(ei => ei.EventId == id && ei.UserId == userId));
    }

    [Authorize]
    [HttpPost("{id}/interest")]
    public async Task<ActionResult> SignalInterest(Guid id, SignalEventInterestRequest req)
    {
        Guid userId = GetCurrentUserId();

        if (!await _dbContext.Events.AnyAsync(e => e.Id == id))
            return NotFound("Event not found");

        if (!await _dbContext.Users.AnyAsync(u => u.Id == userId))
            return BadRequest("Invalid user");

        if (await _dbContext.EventInterests.AnyAsync(ei => ei.EventId == id && ei.UserId == userId))
            return Conflict("User has already signaled interest for this event");

        EventInterest interest = new(userId, id, req.Message);

        _dbContext.EventInterests.Add(interest);
        await _dbContext.SaveChangesAsync();

        return NoContent();

    }

    [Authorize]
    [HttpDelete("{id}/interest")]
    public async Task<ActionResult> RemoveInterest(Guid id)
    {
        Guid userId = GetCurrentUserId();

        var interest = await _dbContext.EventInterests
            .FirstOrDefaultAsync(ei => ei.EventId == id && ei.UserId == userId);

        if (interest is null)
            return NotFound("Interest not found");

        _dbContext.EventInterests.Remove(interest);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id}/image")]
    public async Task<ActionResult> UploadEventImage(Guid id, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        if (file.Length > 5 * 1024 * 1024)
            return BadRequest("File too large.");

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType))
            return BadRequest("Invalid image type.");

        var ev = await _dbContext.Events.FindAsync(id);
        if (ev is null)
            return NotFound("Event not found.");

        var fileName = await _imageStorage.SaveEventImage(id, file);

        ev.SetImage(fileName);
        await _dbContext.SaveChangesAsync();

        return Ok(new { fileName });
    }

    private Guid GetCurrentUserId()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
