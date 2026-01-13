using System.ComponentModel.DataAnnotations;

namespace GoTogether.Application.DTOs.Events;

public record CreateEventRequest(

    [Required][StringLength(200)] string Title,
    [StringLength(1500)] string? Description,
    [Required] DateTime StartsAt,
    [Required][StringLength(100)] string Location,
    [Required][StringLength(15)] string Category
);
