using System.Text;

namespace KiloDb.Core;

public sealed class CommandProcessor(IKeyValueStore store)
{
    public const string HelpText = 
    """
    Commands:
    set <key> <value> store a value (the value may contain spaces)
    get <key> read a value
    del <key> delete a key
    scan [from] [to] list keys in order (use * for "no limit")
    stats engine statistics
    help this text
    exit leave the shell
    """;

    public string Execute(string line)
    {
        try
        {
            return Run(line.Trim());
        }
        catch(Exception ex) when (ex is ArgumentException or InvalidOperationException
                                    or InvalidDataException or IOException)
        {
            return $"ERR {ex.Message}";
        }
    }

    private string Run(string line)
    {
        string[] parts = line.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries); // user could write command in spaces ex. SET   USER   FROM  ...
        if(parts.Length == 0) return "ERR empty command";

        switch (parts[0].ToLowerInvariant())
        {
            case "help":
                return HelpText.TrimEnd();
            case "set" when parts.Length == 3:
                store.Put(parts[1], parts[2]);
                return "OK";
            case "get" when parts.Length == 2:
                string? value = store.Get(parts[1]);
                return value is null ? "(nil)" : $"\"{value}\"";
            case "del" when parts.Length == 2: 
                return store.Delete(parts[1])? "(deleted 1)" : "(deleted 0)";
            case "scan" :
                return Scan(parts);
            case "stats":
                return store.GetStats();
            default:
                return "ERR Unknown command arguments";

        }
    }
    // scan apple date
    private string Scan(string[] parts)
    {
        string? from = parts.Length > 1 && parts[1] != "*"  ? parts[1] : null;
        string? to = parts.Length > 2 && parts[2] != "*" ? parts[2] : null;

        var output = new StringBuilder();

        foreach( var (key, value) in store.Scan(from, to))
        {
            output.Append($"{key} = \"{value}\"\n");
            
        }
        return output.ToString();
    }
}