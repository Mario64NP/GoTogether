namespace GoTogether.Application.DTOs.Events;

public record EventListItemResponse(
    Guid Id,
    string Title,
    DateTime StartsAt,
    string Location,
    string Category,
    string? ImageUrl,
    int InterestedCount
);
