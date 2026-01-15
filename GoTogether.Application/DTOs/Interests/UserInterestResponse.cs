namespace GoTogether.Application.DTOs.Interests;

public record UserInterestResponse
(
    string? AvatarUrl,
    string Username,
    string DisplayName,
    string? Message,
    DateTime InterestedAt,
    Guid EventId,
    string Title,
    DateTime StartsAt,
    string Location,
    string Category,
    string? ImageUrl,
    int InterestedCount
);
