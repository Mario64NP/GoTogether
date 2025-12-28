namespace GoTogether.API.Contracts.Interests;

    public record EventInterestResponse
    (
    string? AvatarUrl,
        string Username,
        string DisplayName,
        string Message,
        DateTime InterestedAt
    );
