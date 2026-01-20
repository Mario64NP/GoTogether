using GoTogether.Application.Abstractions;
using GoTogether.Application.DTOs.Auth;
using GoTogether.Application.DTOs.Common;
using GoTogether.Application.Services.Interfaces;
using GoTogether.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace GoTogether.Infrastructure.Identity;

public class IdentityService(IUserRepository users, IPasswordHasher<User> hasher, IAuthTokenService jwt, IImagePathService paths, IEmailService emails, IConfiguration config) : IIdentityService
{
    private readonly string _baseUrl = Environment.GetEnvironmentVariable("VS_TUNNEL_URL")?.TrimEnd('/') ?? config["BASE_URL"] ?? "https://localhost:7271";

    public async Task<Result> RegisterAsync(RegisterRequest req)
    {
        if (await users.GetUserByUsernameAsync(req.Username) is not null)
            return Result.Failure("Username already taken", ErrorType.Conflict);

        if (await users.GetUserByEmailAsync(req.Email) is not null)
            return Result.Failure("Email already taken", ErrorType.Conflict);

        User u = new(req.Username, req.DisplayName, req.Email);

        u.SetPassword(hasher.HashPassword(u, req.Password));
        u.SetVerificationToken(GenerateToken());
        
        await users.AddUserAsync(u);
        await users.SaveChangesAsync();

        await SendVerificationEmail(u);

        return Result.Success();
    }

    public async Task<Result> VerifyEmailAsync(Guid userId, string token)
    {
        var user = await users.GetUserByIdAsync(userId);
        
        if (user is null)
            return Result.Failure("User not found", ErrorType.NotFound);

        if (user.EmailVerificationToken == token)
        {
            user.VerifyEmail();
            await users.SaveChangesAsync();

            return Result.Success();
        }

        return Result.Failure("Invalid verification token", ErrorType.Failure);
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest req)
    {
        User? u = await users.GetUserByUsernameAsync(req.Username);
        if (u is null || hasher.VerifyHashedPassword(u, u.PasswordHash, req.Password) == PasswordVerificationResult.Failed)
            return Result<AuthResponse>.Failure("Invalid username or password", ErrorType.Unauthorized);

        if (!u.IsEmailVerified)
        {
            u.SetVerificationToken(GenerateToken());
            await users.SaveChangesAsync();
            await SendVerificationEmail(u);

            return Result<AuthResponse>.Failure("Email not verified, please check your inbox", ErrorType.Forbidden);
        }

        var token = jwt.GenerateToken(u.Id, u.Username, u.Role.ToString());

        return Result<AuthResponse>.Success(new AuthResponse(u.Id, u.Username, u.DisplayName, paths.GetAvatarPath(u.AvatarFileName), token));
    }
    static string GenerateToken() => Convert.ToHexString(RandomNumberGenerator.GetBytes(32));

    private async Task SendVerificationEmail(User user)
    {
        var verifyLink = $"{_baseUrl}/api/Auth/verify?userId={user.Id}&token={user.EmailVerificationToken}";
        await emails.SendVerificationEmailAsync(user.Email, user.DisplayName, verifyLink);
    }
}
