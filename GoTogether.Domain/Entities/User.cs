using GoTogether.Domain.Common;

namespace GoTogether.Domain.Entities
{
    public class User(string Username, string DisplayName) : Entity
    {
        public string Username { get; private set; } = Username;
        public string DisplayName { get; private set; } = DisplayName;
        public string PasswordHash { get; private set; } = string.Empty;
        public UserRole Role { get; private set; } = UserRole.User;

        public ICollection<EventInterest> EventInterests { get; private set; } = [];

        public void SetPassword(string hash)
        {
            PasswordHash = hash;
        }

        public void PromoteToAdmin()
        {
            Role = UserRole.Admin;
        }
    }

    public enum UserRole
    {
        User,
        Admin
    }
}
