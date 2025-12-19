using GoTogether.Domain.Common;

namespace GoTogether.Domain.Entities
{
    public class User : Entity
    {
        public string Username { get; private set; } = null!;
        public string DisplayName { get; private set; } = null!;

        public ICollection<EventInterest> EventInterests { get; private set; } = [];
    }
}
