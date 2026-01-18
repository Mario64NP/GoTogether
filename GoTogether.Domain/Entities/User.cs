using GoTogether.Domain.Common;

namespace GoTogether.Domain.Entities;

public class User(string Username, string DisplayName) : Entity
{
    public string Username { get; private set; } = Username;
    public string DisplayName { get; private set; } = DisplayName;
    public string? Bio {  get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string? AvatarFileName { get; private set; } = null;
    public UserRole Role { get; private set; } = UserRole.User;
    public IEnumerable<string> Tags { get; private set; } = [];

    public ICollection<EventInterest> EventInterests { get; private set; } = [];

    public void SetPassword(string hash) => PasswordHash = hash;

    public void PromoteToAdmin() => Role = UserRole.Admin;

    public void DemoteToUser() => Role = UserRole.User;

    public void SetAvatar(string fileName) => AvatarFileName = fileName;

    public void RemoveAvatar() => AvatarFileName = null;

    public void SetEmail(string email) => Email = email;

    public void Update(string? displayname, string? bio, IEnumerable<string>? tags)
    {
        DisplayName = displayname ?? DisplayName;
        Bio = bio ?? string.Empty;
        Tags = tags ?? Tags;
    }
}

public enum UserRole
{
    User,
    Admin
}
