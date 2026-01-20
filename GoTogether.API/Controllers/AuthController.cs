using GoTogether.Application.DTOs.Auth;
using GoTogether.Application.DTOs.Common;
using GoTogether.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace GoTogether.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IIdentityService identityService) : ControllerBase
{
    [HttpPost("register")]
    [EnableRateLimiting("strict")]
    public async Task<ActionResult> Register(RegisterRequest req)
    {
        var result = await identityService.RegisterAsync(req);

        return result.IsSuccess ? Ok() : HandleError(result);
    }

    [HttpGet("verify")]
    [EnableRateLimiting("strict")]
    public async Task<ActionResult> VerifyEmail([FromQuery] Guid userId, [FromQuery] string token)
    {
        var result = await identityService.VerifyEmailAsync(userId, token);

        return result.IsSuccess ? Ok("Email verified! You can log in now.") : HandleError(result);
    }

    [HttpPost("login")]
    [EnableRateLimiting("strict")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest req)
    {
        var result = await identityService.LoginAsync(req);

        return result.IsSuccess ? Ok(result.Value) : HandleError(result);
    }

    private ObjectResult HandleError(Result result) => result.ErrorType switch
    {
        ErrorType.Conflict => Conflict(result.Error),
        ErrorType.Unauthorized => Unauthorized(result.Error),
        ErrorType.Forbidden => StatusCode(403, result.Error),
        ErrorType.NotFound => NotFound(result.Error),
        _ => BadRequest(result.Error)
    };
}
