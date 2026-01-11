using GoTogether.Application.DTOs.Auth;
using GoTogether.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoTogether.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IIdentityService identityService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest req)
    {
        // validate register req

        var result = await identityService.RegisterAsync(req);

        if (result is null)
            return Conflict("Username already taken");

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest req)
    {
        var result = await identityService.LoginAsync(req);

        if (result is null)
            return Unauthorized("Invalid username or password");

        return Ok(result);
    }
}
