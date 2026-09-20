using KiloDb.Core;
namespace KiloDb.Cli;

public sealed class Repl(IKeyValueStore store)
{
    public void Run()
    {
        var processor = new CommandProcessor(store);
        Console.WriteLine($"KiloDb shell (engine: {store.EngineName}). Type 'help'.");

        while (true)
        {
            Console.Write("Kilo> ");
            string? line = Console.ReadLine();
            if(line is null)break;
            line = line.Trim();
            if(line.Length == 0)continue;
            if(line is "exit" or "quit") break;

            Console.WriteLine(processor.Execute(line));
        }
    }
}