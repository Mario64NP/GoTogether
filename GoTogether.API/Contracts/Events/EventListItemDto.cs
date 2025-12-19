namespace GoTogether.API.Contracts.Events
{
    public record EventListItemDto(
        Guid Id,
        string Title,
        DateTime StartsAt,
        string Location,
        int InterestedCount
    );
}
