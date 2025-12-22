namespace GoTogether.API.Contracts.Users
{
    public record UserDetailsDto
    (
        Guid Id,
        string Username,
        string DisplayName
    );
}
