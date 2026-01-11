using GoTogether.Application.DTOs.Auth;

namespace GoTogether.Application.Services.Interfaces;

public interface IIdentityService
{
    Task<AuthResponse?> RegisterAsync(RegisterRequest req);
    Task<AuthResponse?> LoginAsync(LoginRequest req);
}
