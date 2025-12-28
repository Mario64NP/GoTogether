using GoTogether.API.Contracts.Users;
using GoTogether.Infrastructure.Files;
using GoTogether.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoTogether.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(GoTogetherDbContext goTogetherDbContext, IImagePathService paths, IImageStorageService storage) : ControllerBase
{
    private readonly GoTogetherDbContext _dbContext = goTogetherDbContext;
    private readonly IImagePathService _imagePaths = paths;
    private readonly IImageStorageService _imageStorage = storage;

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDetailsDto>> GetCurrentUser()
    {
        Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
            return NotFound();

        var userDto = new UserDetailsDto(user.Id, user.Username, user.DisplayName, _imagePaths.GetAvatarImagePath(user.AvatarFileName));

        return Ok(userDto);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<UserDetailsDto>> GetUserByUsername(string username)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user is null)
            return NotFound();

        var userDto = new UserDetailsDto(user.Id, user.Username, user.DisplayName, _imagePaths.GetAvatarImagePath(user.AvatarFileName));

        return Ok(userDto);
    }

    [Authorize]
    [HttpPost("avatar")]
    public async Task<ActionResult> UploadAvatar(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        if (file.Length > 5 * 1024 * 1024)
            return BadRequest("File too large.");

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType))
            return BadRequest("Invalid image type.");

        Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
            return NotFound("User not found.");

        var fileName = await _imageStorage.SaveProfileAvatar(userId, file);

        user.SetAvatar(fileName);
        await _dbContext.SaveChangesAsync();

        return Ok(new { fileName });
    }
}
