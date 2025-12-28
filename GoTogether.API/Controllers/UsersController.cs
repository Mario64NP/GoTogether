using GoTogether.API.Contracts.Users;
using GoTogether.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoTogether.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(GoTogetherDbContext goTogetherDbContext) : ControllerBase
    {
        private readonly GoTogetherDbContext _dbContext = goTogetherDbContext;

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDetailsDto>> GetCurrentUser()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
                return NotFound();

            var userDto = new UserDetailsDto(user.Id, user.Username, user.DisplayName);

            return Ok(userDto);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<UserDetailsDto>> GetUserByUsername(string username)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user is null)
                return NotFound();

            var userDto = new UserDetailsDto(user.Id, user.Username, user.DisplayName);

            return Ok(userDto);
        }
    }
}
