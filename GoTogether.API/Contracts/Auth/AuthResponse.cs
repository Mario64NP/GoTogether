namespace GoTogether.API.Contracts.Auth
{
    public record AuthResponse
    (
        Guid UserId,
        string Username,
        string DisplayName,
        string Token
    );
}
