namespace GoTogether.API.Contracts.Events
{
    public record EventListItemDto(
        Guid Id,
        string Title,
        DateTime StartsAt,
        string Location,
        string ImageUrl,
        int InterestedCount
    );
}
