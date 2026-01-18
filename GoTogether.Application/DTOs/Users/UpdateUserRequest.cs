using System.ComponentModel.DataAnnotations;

namespace GoTogether.Application.DTOs.Users;

public record UpdateUserRequest
(
    [StringLength(50)] string? DisplayName,
    [StringLength(250)] string? Bio,
    IEnumerable<string>? Tags
);
