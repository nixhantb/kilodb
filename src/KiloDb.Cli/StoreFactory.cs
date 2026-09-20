using KiloDb.Core;

namespace KiloDb.Cli;

public static class StoreFactory{
    public static readonly String[] EngineNames = ["memory"];

    public static IKeyValueStore Open(string engine, string folder) => engine switch
    {
        "memory" => new MemoryStore(),
        _ => throw new ArgumentException(
            $"Unknown engine '{engine}'. Available: {string.Join(", ", EngineNames)})"
        )
    };
}