namespace GoTogether.Application.DTOs.Auth;

public record AuthResponse
(
    Guid UserId,
    string Username,
    string DisplayName,
    string? AvatarUrl,
    string Token
);
