using KiloDb.Core;
namespace KiloDb.Cli.Demos;

public sealed class step01Demos : IDemo
{
    public string Command => "step01";
    public string Description => "In-memory store and the command processor";

    public void Run(string[] args)
    {
        Ui.Title("step 1: an in-memory key-value store");
        using var store = new MemoryStore();

        var shell = new CommandProcessor(store);
        string[] script =
            [
            "set user:1 Alice",
            "set user:2 Bob",
            "set user:3 Carol Smith",
            "get user:2",
            "set user:2 Bobby",
            "get user:2",
            "del user:1",
            "get user:1",
            "scan",
            "stats",
            ];
    
    foreach(string line in script)
        {
            Console.WriteLine($"kilo> {line}");
            Console.WriteLine(shell.Execute(line));

        }
        Ui.Section("The catch");
        Ui.Warn("All of this lives in RAM. Stop the program and it is gone.");
        Ui.Info("Steps 2-4 fix that by writing to a file.");
        
    }
}