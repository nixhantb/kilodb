using System.Diagnostics;
using System.Reflection;
using KiloDb.Tests;
// Usage: dotnet run -> run every test
// dotnet run -- Step08 -> run only tests whose class or method name contains "Step08"
string? filter = args.FirstOrDefault();
var tests = typeof(TestAttribute).Assembly.GetTypes()
    .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static))
    .Where(m => m.GetCustomAttribute<TestAttribute>() is not null)
    .Where(m => filter is null || m.DeclaringType!.Name.Contains(filter, StringComparison.OrdinalIgnoreCase)
    || m.Name.Contains(filter, StringComparison.OrdinalIgnoreCase))
    .OrderBy(m => m.DeclaringType!.Name, StringComparer.Ordinal)
    .ThenBy(m => m.Name, StringComparer.Ordinal)
    .ToList();



int passed = 0, failed = 0;
var total = Stopwatch.StartNew();
foreach (var test in tests)
{
    string name = $"{test.DeclaringType!.Name}.{test.Name}";
    var timer = Stopwatch.StartNew();
    try
    {
        test.Invoke(null, null);
        passed++;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" PASS {name} ({timer.ElapsedMilliseconds} ms)");
    }
    catch (TargetInvocationException ex)
    {
        failed++;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($" FAIL {name}");
        Exception? inner = ex.InnerException;
        Console.WriteLine($" {inner?.GetType().Name}: {inner?.Message}");
    }
    Console.ResetColor();
}
Console.WriteLine();
Console.WriteLine($"{passed} passed, {failed} failed, {tests.Count} total " +
$"({total.ElapsedMilliseconds} ms)");

return failed == 0? 0: 1;