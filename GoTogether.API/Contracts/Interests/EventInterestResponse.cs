namespace GoTogether.API.Contracts.Interests
{
    public record EventInterestResponse
    (
        string Username,
        string DisplayName,
        string Message,
        DateTime InterestedAt
    );
}
