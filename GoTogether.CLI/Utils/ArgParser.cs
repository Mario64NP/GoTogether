namespace GoTogether.CLI.Utils;

public class ArgParser
{
     public static string Get(string[] args, string key)
    {
        var index = args.IndexOf(key);
        var result = args.ElementAt(index + 1);
        return result;
    }
}
