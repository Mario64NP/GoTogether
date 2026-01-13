using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace GoTogether.API.Authorization;

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
