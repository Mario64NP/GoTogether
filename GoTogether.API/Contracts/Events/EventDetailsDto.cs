namespace GoTogether.API.Contracts.Events;

public record EventDetailsDto(
    Guid Id,
    string Title,
    string Description,
    DateTime StartsAt,
    string Location,
    string? ImageUrl,
    int InterestedCount
);
