using GoTogether.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GoTogether.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        return services;
    }

    public static IServiceCollection AddDevAuthLogging(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, LogFailureHandler>();

        return services;
    }

    public static IServiceCollection AddInfrastructurePaths(this IServiceCollection services)
    {
        var basePath = Environment.GetEnvironmentVariable("GOTOGETHER_DATA_DIR") ?? AppContext.BaseDirectory;

        services.AddSingleton(new InfrastructurePaths(basePath));

        return services;
    }
}

public class LogFailureHandler : AuthorizationHandler<RolesAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesAuthorizationRequirement requirement)
    {
        var hasRole = requirement.AllowedRoles.Any(r => context.User.IsInRole(r));

        if (!hasRole)
        {
            var user = context.User.Identity?.Name ?? "Anonymous";
            var roles = context.User.Claims
                .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role || c.Type == "role")
                .Select(c => c.Value).ToList();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n=========================================================");
            Console.WriteLine("[AUTHORIZATION FAILURE]");
            Console.WriteLine($"User: {user} | Path: {((DefaultHttpContext)context.Resource!).Request.Path}");
            Console.WriteLine($"Required: {string.Join(", ", requirement.AllowedRoles)} | Actual: {(roles.Any() ? string.Join(", ", roles) : "NONE")}");
            Console.WriteLine("=========================================================\n");
            Console.ResetColor();
        }

        return Task.CompletedTask;
    }
}