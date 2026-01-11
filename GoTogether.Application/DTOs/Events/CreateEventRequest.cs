namespace GoTogether.Application.DTOs.Events;

public record CreateEventRequest(

    string Title,
    string Description,
    DateTime StartsAt,
    string Location,
    string Category
);
