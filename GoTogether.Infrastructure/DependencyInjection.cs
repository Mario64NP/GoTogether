using GoTogether.Application.Abstractions;
using GoTogether.Application.Services.Interfaces;
using GoTogether.Domain.Entities;
using GoTogether.Infrastructure.Files;
using GoTogether.Infrastructure.Identity;
using GoTogether.Infrastructure.Persistence;
using GoTogether.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GoTogether.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<GoTogetherDbContext>((sp, options) =>
        {
            var paths = sp.GetRequiredService<InfrastructurePaths>();
            var dbPath = Path.Combine(paths.Root, "GoTogether.db");

            options.UseSqlite($"Data Source={dbPath};Cache=Shared;Mode=ReadWriteCreate;");
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEventRepository, EventRepository>();

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IAuthTokenService, AuthTokenService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        services.AddScoped<IImagePathService, ImagePathService>();
        services.AddScoped<IImageStorageService, ImageStorageService>();

        return services;
    }
}
