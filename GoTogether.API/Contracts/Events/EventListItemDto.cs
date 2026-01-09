namespace GoTogether.API.Contracts.Events;

public record EventListItemDto(
    Guid Id,
    string Title,
    DateTime StartsAt,
    string Location,
    string Category,
    string? ImageUrl,
    int InterestedCount
);
