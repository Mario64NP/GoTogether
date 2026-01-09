namespace GoTogether.API.Contracts.Events;

public record CreateEventRequest(

    string Title,
    string Description,
    DateTime StartsAt,
    string Location,
    string Category
);
