namespace GoTogether.API.Contracts.Auth
{
    public record LoginRequest
    (
        string Username,
        string Password
    );
}
