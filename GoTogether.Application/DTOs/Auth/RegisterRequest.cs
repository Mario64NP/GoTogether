namespace GoTogether.Application.DTOs.Auth;

public record RegisterRequest
(
    string Username,
    string DisplayName,
    string Password
);
