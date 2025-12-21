using GoTogether.API.Contracts.Events;
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
    }
}
