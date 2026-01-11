using GoTogether.Application.Services.Interfaces;
using GoTogether.CLI.Utils;

namespace GoTogether.CLI.Commands;

public class EventCommands(IEventService eventService)
{
    public async Task ExecuteAsync(string action, string[] args)
    {
        switch (action)
        {
            case "remove-interest":
                await RemoveInterest(args);
                break;

            default:
                throw new Exception($"Unknown action {action}");
        }
    }

    public async Task RemoveInterest(string[] args)
    {
        bool isAll = args.Length == 3 && args[0].Equals("all", StringComparison.OrdinalIgnoreCase);
        bool isSingle = args.Length == 4;

        if (!isAll && !isSingle)
        {
            Console.WriteLine("Invalid arguments.");
            return;
        }

        var eventId = Guid.Parse(ArgParser.Get(args, "--eventId"));
        var userId = isAll ? Guid.Empty : Guid.Parse(ArgParser.Get(args, "--userId"));

        int rows = await eventService.RemoveInterestAsync(userId, eventId);
        Console.WriteLine($"Removed {rows} interests.");
    }
}
