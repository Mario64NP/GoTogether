using GoTogether.API.Contracts.Auth;
using GoTogether.API.Services.Auth;
using GoTogether.Domain.Entities;
using GoTogether.Infrastructure.Files;
using GoTogether.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoTogether.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(GoTogetherDbContext goTogetherDbContext, IPasswordHasher<User> passwordHasher, AuthTokenService authTokenService, IImagePathService path) : ControllerBase
{
    private readonly GoTogetherDbContext _dbContext = goTogetherDbContext;
    private readonly IPasswordHasher<User> _hasher = passwordHasher;
    private readonly AuthTokenService _jwt = authTokenService;
    private readonly IImagePathService _paths = path;

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest req)
    {
        if (await _dbContext.Users.AnyAsync(u => u.Username == req.Username))
            return Conflict("Username already taken");

        User u = new(req.Username, req.DisplayName);
        u.SetPassword(_hasher.HashPassword(u, req.Password));

        _dbContext.Users.Add(u);
        await _dbContext.SaveChangesAsync();

        var token = _jwt.GenerateToken(u.Id, u.Username, u.Role.ToString());

        return Ok(new AuthResponse(u.Id, u.Username, u.DisplayName, _paths.GetAvatarImagePath(u.AvatarFileName), token));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest req)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == req.Username);

        if (user is null || _hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password) == PasswordVerificationResult.Failed)
            return Unauthorized("Invalid username or password");

        var token = _jwt.GenerateToken(user.Id, user.Username, user.Role.ToString());

        return Ok(new AuthResponse(user.Id, user.Username, user.DisplayName, _paths.GetAvatarImagePath(user.AvatarFileName), token));
    }

}
