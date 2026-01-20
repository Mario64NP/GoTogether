using GoTogether.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace GoTogether.Domain.Entities;

public class User(string username, string displayName, string email) : Entity
{
    public string Username { get; private set; } = username;
    public string DisplayName { get; private set; } = displayName;
    public string? Bio {  get; private set; }
    public string Email { get; private set; } = email;
    public bool IsEmailVerified { get; private set; }
    public string? EmailVerificationToken { get; private set; }
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

    public bool SetEmail(string email)
    {
        EmailAddressAttribute detector = new();
        if (detector.IsValid(email))
        {
            Email = email;
            IsEmailVerified = false;

            return true;
        }

        return false;
    }

    public void VerifyEmail()
    {
        IsEmailVerified = true;
        EmailVerificationToken = null;
    }

    public void SetVerificationToken(string token) => EmailVerificationToken = token;

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
