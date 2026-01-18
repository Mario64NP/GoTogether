namespace GoTogether.Application.DTOs.Users;

public record UserDetailsResponse
(
    Guid Id,
    string Username,
    string DisplayName,
    string? Bio,
    IEnumerable<string> Tags,
    string? AvatarUrl
);
