using GoTogether.Application.Abstractions;
using GoTogether.Application.DTOs.Auth;
using GoTogether.Application.Services.Interfaces;
using GoTogether.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GoTogether.Infrastructure.Identity;

public class IdentityService(IUserRepository users, IPasswordHasher<User> hasher, IAuthTokenService jwt, IImagePathService paths) : IIdentityService
{
    public async Task<AuthResponse?> RegisterAsync(RegisterRequest req)
    {
        if (await users.GetUserByUsernameAsync(req.Username) is not null)
            return null;

        User u = new(req.Username, req.DisplayName);
        u.SetPassword(hasher.HashPassword(u, req.Password));

        await users.AddUserAsync(u);
        await users.SaveChangesAsync();

        var token = jwt.GenerateToken(u.Id, u.Username, u.Role.ToString());
        return new AuthResponse(u.Id, u.Username, u.DisplayName, paths.GetAvatarPath(u.AvatarFileName), token);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest req)
    {
        User? u = await users.GetUserByUsernameAsync(req.Username);
        if (u is null || hasher.VerifyHashedPassword(u, u.PasswordHash, req.Password) == PasswordVerificationResult.Failed)
            return null;

        var token = jwt.GenerateToken(u.Id, u.Username, u.Role.ToString());
        return new AuthResponse(u.Id, u.Username, u.DisplayName, paths.GetAvatarPath(u.AvatarFileName), token);
    }
}
