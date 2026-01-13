using System.ComponentModel.DataAnnotations;

namespace GoTogether.Application.DTOs.Events;

public record UpdateEventRequest
(
    [StringLength(200)] string? Title,
    [StringLength(1500)] string? Description,
    DateTime? StartsAt,
    [StringLength(100)] string? Location,
    [StringLength(15)] string? Category
);