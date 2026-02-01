using GoTogether.Application.Services.Interfaces;
using GoTogether.CLI.Utils;

namespace GoTogether.CLI.Commands;

public class UserCommands(IUserService userService)
{
    public async Task ExecuteAsync(string action,  string[] args)
    {
        switch (action)
        {
            case "set-email":
                await SetEmailAsync(args);
                break;

            case "set-role":
                await SetRoleAsync(args);
                break;

            default:
                Console.WriteLine($"Unknown user action: {action}");
                break;
        }
    }

    public async Task SetEmailAsync(string[] args)
    {
        var username = ArgParser.Get(args, "--username");
        var email = ArgParser.Get(args, "--email");

        if (string.IsNullOrWhiteSpace(username) ||  string.IsNullOrWhiteSpace(email))
        {
            Console.WriteLine("Invalid username or email.");
            return;
        }

        if (await userService.SetEmailAsync(username, email))
            Console.WriteLine($"{username}'s email has been changed to {email}.");
        else
            Console.WriteLine("Couldn't change the user's email.");
    }

    public async Task SetRoleAsync(string[] args)
    {
        var username = ArgParser.Get(args, "--username");
        var role = ArgParser.Get(args, "--role");

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(role))
        {
            Console.WriteLine("Invalid username or role.");
            return;
        }

        if (await userService.SetRoleAsync(username, role))
            Console.WriteLine($"{username} has been granted the {role} role.");
        else
            Console.WriteLine("Couldn't set the user's role.");
    }
}
