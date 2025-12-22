using GoTogether.API.Contracts.Events;
using GoTogether.API.Contracts.Interests;
using GoTogether.Domain.Entities;
using GoTogether.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoTogether.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly GoTogetherDbContext _dbContext;

        public EventsController(GoTogetherDbContext dbContext)
        {
            _dbContext = dbContext;
        }

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
                    e.EventInterests.Count
                ))
                .ToListAsync();
        }

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
                    e.EventInterests.Count
                ))
                .FirstOrDefaultAsync();

            if (ev is null)
                return NotFound();

            return Ok(ev);
        }

        [HttpPost]
        public async Task<ActionResult<EventDetailsDto>> CreateEvent(CreateEventRequest req)
        {
            Event e = new(req.Title, req.StartsAt, req.Location, req.Description);

            _dbContext.Events.Add(e);
            await _dbContext.SaveChangesAsync();

            var evDto = new EventDetailsDto(
                e.Id,
                e.Title,
                e.Description,
                e.StartsAt,
                e.Location,
                e.EventInterests.Count
            );

            return CreatedAtAction(
                nameof(GetEventById),
                new { id = e.Id },
                evDto
            );
        }

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
                req.Location
            );

            await _dbContext.SaveChangesAsync();

            var evDto = new EventDetailsDto(
                ev.Id,
                ev.Title,
                ev.Description,
                ev.StartsAt,
                ev.Location,
                ev.EventInterests.Count
            );

            return Ok(evDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvent(Guid id)
        {
            var ev = await _dbContext.Events.FindAsync(id);

            if (ev is null)
                return NotFound();

            _dbContext.Events.Remove(ev);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/interest")]
        public async Task<ActionResult> SignalInterest(Guid id, SignalEventInterestRequest req, [FromHeader(Name = "X-User-Id")] Guid userId)
        {
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

        [HttpDelete("{id}/interest")]
        public async Task<ActionResult> RemoveInterest(Guid id, [FromHeader(Name = "X-User-Id")] Guid userId)
        {
            var interest = await _dbContext.EventInterests
                .FirstOrDefaultAsync(ei => ei.EventId == id && ei.UserId == userId);

            if (interest is null)
                return NotFound("Interest not found");

            _dbContext.EventInterests.Remove(interest);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
