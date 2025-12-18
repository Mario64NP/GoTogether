using GoTogether.Domain.Common;

namespace GoTogether.Domain.Entities
{
    public class EventInterest : Entity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid EventId { get; set; }
        public Event Event { get; set; } = null!;

        public string? Message { get; set; }
    }
}
