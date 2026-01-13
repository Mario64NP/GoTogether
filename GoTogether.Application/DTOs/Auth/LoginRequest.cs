using System.ComponentModel.DataAnnotations;

namespace GoTogether.Application.DTOs.Auth;

public record LoginRequest
(
    [Required][StringLength(30)] string Username,
    [Required][StringLength(100)] string Password
);
