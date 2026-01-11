using GoTogether.CLI.Commands;

namespace GoTogether.CLI;

public sealed class CommandRunner(UserCommands userCommands, EventCommands eventCommands)
{
    public async Task RunAsync(string[] args)
    {
        if (args.Length < 2)
            Console.WriteLine("Invalid command");

        var domain = args[0];
        var action = args[1];
        var rest = args.Skip(2).ToArray();

        switch (domain)
        {
            case "user":
                await userCommands.ExecuteAsync(action, rest);
                break;
            case "event":
                await eventCommands.ExecuteAsync(action, rest);
                break;

            default: 
                Console.WriteLine($"Unknown domain {domain}");
                break;
        }
    }
}
