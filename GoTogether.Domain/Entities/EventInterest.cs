using GoTogether.Domain.Common;

namespace GoTogether.Domain.Entities;

public class EventInterest(Guid UserId, Guid EventId, string? Message) : Entity
{
    public Guid UserId { get; set; } = UserId;
    public User User { get; set; } = null!;

    public Guid EventId { get; set; } = EventId;
    public Event Event { get; set; } = null!;

    public string? Message { get; set; } = Message;
}
