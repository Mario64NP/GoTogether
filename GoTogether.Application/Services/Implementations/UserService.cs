using GoTogether.Application.Abstractions;
using GoTogether.Application.DTOs.Files;
using GoTogether.Application.DTOs.Users;
using GoTogether.Application.Services.Interfaces;
using GoTogether.Domain.Entities;

namespace GoTogether.Application.Services.Implementations;

public class UserService(IUserRepository users, IImagePathService paths, IImageStorageService storage) : IUserService
{
    public async Task<UserDetailsResponse?> GetUserByIdAsync(Guid userId) => MapToDto(await users.GetUserByIdAsync(userId));

    public async Task<UserDetailsResponse?> GetUserByUsernameAsync(string username) => MapToDto(await users.GetUserByUsernameAsync(username));

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

        return fileName;
    }

    private UserDetailsResponse? MapToDto(User? user)
    {
        if (user is null) return null;

        return new UserDetailsResponse(
            user.Id,
            user.Username,
            user.DisplayName,
            paths.GetAvatarPath(user.AvatarFileName));
    }
}
