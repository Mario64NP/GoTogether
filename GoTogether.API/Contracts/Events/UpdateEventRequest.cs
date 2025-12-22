namespace GoTogether.API.Contracts.Events
{
    public record UpdateEventRequest
    (
        string Title,
        string Description,
        DateTime StartsAt,
        string Location
    );
}
