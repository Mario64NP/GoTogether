using GoTogether.Application;
using GoTogether.CLI;
using GoTogether.CLI.Commands;
using GoTogether.Infrastructure;
using GoTogether.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddApplication();
        services.AddInfrastructure();

        services.AddScoped<CommandRunner>();
        services.AddScoped<UserCommands>();
        services.AddScoped<EventCommands>();

        var basePath = Environment.GetEnvironmentVariable("GOTOGETHER_DATA_DIR") ?? AppContext.BaseDirectory;
        services.AddSingleton(new InfrastructurePaths(basePath));
    }).Build();

using var scope = builder.Services.CreateScope();
var services = scope.ServiceProvider;

var db = services.GetRequiredService<GoTogetherDbContext>();
db.Database.Migrate();

var runner = services.GetRequiredService<CommandRunner>();
await runner.RunAsync(args);