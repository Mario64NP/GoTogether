using GoTogether.Domain.Common;

namespace GoTogether.Domain.Entities
{
    public class Event : Entity
    {
        public string Title { get; set; } = null!;
        public DateTime StartsAt { get; set; }
        public string Location { get; set; } = null!;
        public string? Description { get; set; }

        public ICollection<EventInterest> EventInterests { get; private set; } = [];
    }
}
