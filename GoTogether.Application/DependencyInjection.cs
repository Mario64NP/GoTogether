using GoTogether.Application.Services.Implementations;
using GoTogether.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GoTogether.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEventService, EventService>();

        return services;
    }
}