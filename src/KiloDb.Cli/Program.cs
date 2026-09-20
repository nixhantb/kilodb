using KiloDb.Cli;

var demos = typeof(IDemo).Assembly.GetTypes()
.Where(t => typeof(IDemo).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false })
.Select(t => (IDemo)Activator.CreateInstance(t)!)
.OrderBy(d => d.Command, StringComparer.Ordinal)
.ToList();




if (args.Length == 0)
{
    Console.WriteLine("KiloDb - a database built from scratch");
    Console.WriteLine();

    foreach (var d in demos)
    {
        Console.WriteLine($"    {d.Command,-8} {d.Description}");
    }

    return 0;
}

if (args[0] == "repl")
{
    string engine = args.Length > 1 ? args[1] : "memory";
    string folder = args.Length > 2
        ? args[2]
        : Path.Combine("data", "repl-" + engine);

    using var store = StoreFactory.Open(engine, folder);
    new Repl(store).Run();

    return 0;
}

var demo = demos.FirstOrDefault(d => d.Command == args[0]);
if(demo is null)
{
    Console.WriteLine($"Unknown command '{args[0]}'. Run with no arguments to see the list.");
    return 1;
}
demo.Run(args[1..]);
return 0;