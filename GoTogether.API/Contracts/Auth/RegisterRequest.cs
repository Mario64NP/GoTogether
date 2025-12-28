namespace GoTogether.API.Contracts.Auth;

public record RegisterRequest
(
    string Username,
    string DisplayName,
    string Password
);
