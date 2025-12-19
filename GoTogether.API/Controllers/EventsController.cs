using GoTogether.API.Contracts.Events;
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
    }
}
