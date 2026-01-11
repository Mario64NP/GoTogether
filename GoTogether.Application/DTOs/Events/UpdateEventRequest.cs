namespace GoTogether.Application.DTOs.Events;

public record UpdateEventRequest
(
    string? Title,
    string? Description,
    DateTime? StartsAt,
    string? Location,
    string? Category
);