using System.ComponentModel.DataAnnotations;

namespace GoTogether.Application.DTOs.Interests;

public record SignalEventInterestRequest
(
    [StringLength(500)] string? Message
);
