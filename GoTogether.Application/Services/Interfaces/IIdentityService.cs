using GoTogether.Application.DTOs.Auth;
using GoTogether.Application.DTOs.Common;

namespace GoTogether.Application.Services.Interfaces;

public interface IIdentityService
{
    Task<Result> RegisterAsync(RegisterRequest req);
    Task<Result> VerifyEmailAsync(Guid userId, string token);
    Task<Result<AuthResponse>> LoginAsync(LoginRequest req);
}
