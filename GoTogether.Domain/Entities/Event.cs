using GoTogether.Domain.Common;

namespace GoTogether.Domain.Entities
{
    public class Event(string Title, DateTime StartsAt, string Location, string Description) : Entity
    {
        public string Title { get; set; } = Title;
        public DateTime StartsAt { get; set; } = StartsAt;
        public string Location { get; set; } = Location;
        public string Description { get; set; } = Description;

        public ICollection<EventInterest> EventInterests { get; private set; } = [];
    }
}
