using GoTogether.Application.Abstractions;
using GoTogether.Application.DTOs.Files;
using GoTogether.Application.DTOs.Interests;
using GoTogether.Application.DTOs.Users;
using GoTogether.Application.Services.Interfaces;
using GoTogether.Domain.Entities;

namespace GoTogether.Application.Services.Implementations;

public class UserService(IUserRepository users, IImagePathService paths, IImageStorageService storage) : IUserService
{
    public async Task<UserDetailsResponse?> GetUserByIdAsync(Guid userId) => MapToDto(await users.GetUserByIdAsync(userId));

    public async Task<UserDetailsResponse?> GetUserByUsernameAsync(string username) => MapToDto(await users.GetUserByUsernameAsync(username));

    public async Task<UserDetailsResponse?> UpdateUserAsync(Guid userId, UpdateUserRequest req)
    {
        var user = MapToDto(await users.UpdateUserAsync(userId, req.DisplayName, req.Bio, req.Tags));
        await users.SaveChangesAsync();

        return user;
    }

    public async Task<IEnumerable<UserInterestResponse>> GetInterestedEventsByUserAsync(Guid userId)
    {
        var interests = await users.GetInterestedEventsByUserAsync(userId);

        return interests.Select(ei => MapToDto(ei));
    }

    public async Task<bool> SetRoleAsync(string username, string role)
    {
        var user = await users.GetUserByUsernameAsync(username);
        if (user is null)
            return false;

        switch (role.ToLowerInvariant())
        {
            case "admin":
                user.PromoteToAdmin();
                break;

            case "user":
                user.DemoteToUser();
                break;

            default:
                return false;
        }

        await users.SaveChangesAsync();
        return true;
    }

    public async Task<string?> SaveAvatarAsync(Guid userId, FileRequest req)
    {
        var user = await users.GetUserByIdAsync(userId);
        if (user is null)
            return null;

        if (!string.IsNullOrEmpty(user.AvatarFileName))
            await storage.DeleteProfileAvatarAsync(user.AvatarFileName);

        var fileName = await storage.SaveProfileAvatarAsync(userId, req);
        user.SetAvatar(fileName);

        await users.SaveChangesAsync();

        return paths.GetAvatarPath(fileName);
    }

    private UserDetailsResponse? MapToDto(User? user)
    {
        if (user is null) return null;

        return new UserDetailsResponse(
            user.Id,
            user.Username,
            user.DisplayName,
            user.Bio,
            user.Tags,
            paths.GetAvatarPath(user.AvatarFileName));
    }

    private UserInterestResponse MapToDto(EventInterest ei) => new(paths.GetAvatarPath(ei.User.AvatarFileName), ei.User.Username, ei.User.DisplayName, ei.Message, ei.CreatedAt,
            ei.Event.Id, ei.Event.Title, ei.Event.StartsAt, ei.Event.Location, ei.Event.Category, paths.GetEventImagePath(ei.Event.ImageFileName), ei.Event.EventInterests.Count);
}
