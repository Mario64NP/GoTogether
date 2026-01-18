using GoTogether.Infrastructure.Persistence;
using Microsoft.Extensions.FileProviders;

namespace GoTogether.API.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseApiSecurityConfiguration(this IApplicationBuilder app)
    {
        app.UseForwardedHeaders();
        app.UseExceptionHandler();
        app.UseRateLimiter();

        return app;
    }

    public static IApplicationBuilder UseImageLogging(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            if (context.Request.Path.StartsWithSegments("/uploads/images"))
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"[IMAGE REQUEST]: {context.Request.Path}");
                Console.ResetColor();
            }
            await next();
        });
    }

    public static IApplicationBuilder UseUploadsStaticFiles(this IApplicationBuilder app)
    {
        var paths = app.ApplicationServices.GetRequiredService<InfrastructurePaths>();

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(paths.Root, "uploads")),
            RequestPath = "/uploads",
            OnPrepareResponse = ctx =>
            {
                ctx.Context.Response.Headers.CacheControl = $"public, no-cache";

                ctx.Context.Response.Headers.Remove("Pragma");
                ctx.Context.Response.Headers.Remove("Expires");

                ctx.Context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
                ctx.Context.Response.Headers.Append("X-Frame-Options", "DENY");
            }
        });

        return app;
    }
}
