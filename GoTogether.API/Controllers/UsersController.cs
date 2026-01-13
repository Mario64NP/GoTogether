using GoTogether.API.Extensions;
using GoTogether.Application.DTOs.Files;
using GoTogether.Application.DTOs.Users;
using GoTogether.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace GoTogether.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDetailsResponse>> GetCurrentUser()
    {
        Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var user = await userService.GetUserByIdAsync(userId);

        if (user is null)
            return NotFound();

        return Ok(user);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<UserDetailsResponse>> GetUserByUsername(string username)
    {
        var user = await userService.GetUserByUsernameAsync(username);

        if (user is null)
            return NotFound();

        return Ok(user);
    }

    [Authorize]
    [HttpPost("avatar")]
    [EnableRateLimiting("strict")]
    public async Task<ActionResult> UploadAvatar(IFormFile file)
    {
        if (!await file.IsValidImageAsync())
            return BadRequest("Invalid image. Please upload a JPEG, PNG, or WebP under 5MB.");

        Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        using var stream = file.OpenReadStream();

        var req = new FileRequest(file.FileName, file.ContentType, stream);

        var fileName = await userService.SaveAvatarAsync(userId, req);

        return Ok(new { fileName });
    }
}
