namespace GoTogether.Application.DTOs.Events;

public record EventDetailsResponse(
    Guid Id,
    string Title,
    string? Description,
    DateTime StartsAt,
    string Location,
    string Category,
    string? ImageUrl,
    int InterestedCount
);
