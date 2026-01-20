using System.ComponentModel.DataAnnotations;

namespace GoTogether.Application.DTOs.Auth;

public record RegisterRequest
(
    [Required][StringLength(30)] string Username,
    [Required][StringLength(50)] string DisplayName,
    [Required, EmailAddress][StringLength(254)] string Email,
    [Required][StringLength(100)] string Password
);
