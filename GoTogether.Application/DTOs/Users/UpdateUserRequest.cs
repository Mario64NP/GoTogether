namespace GoTogether.Application.DTOs.Users;

public record UpdateUserRequest
(
    string? DisplayName,
    string? Bio,
    IEnumerable<string>? Tags
);
