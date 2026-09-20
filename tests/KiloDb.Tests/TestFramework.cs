namespace KiloDb.Tests;

[AttributeUsage(AttributeTargets.Method)]
public sealed class TestAttribute : Attribute;

public sealed class AssertException(string message) : Exception(message);


public static class Assert
{
    public static void True(bool condition, string message = "expected true")
    {
        if (!condition) throw new AssertException(message);
    }
    public static void False(bool condition, string message = "expected false")
    {
        if (condition) throw new AssertException(message);
    }
    public static void Equal<T>(T expected, T actual, string? context = null)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
        {
            throw new AssertException(
                $"{context} {(context is null ? "" : ": ")}"
                + $"expected <{expected}> but got <{actual}>"
            );
        }
    }

    public static void Null(object? value, string? context = null)
    {
        if (value is not null)
        {
            throw new AssertException($"{context} expected null, got <{value}>");
        }

    }

    public static void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
    {
        var e = expected.ToList();
        var a = actual.ToList();
        Equal(e.Count, a.Count, "sequence length");
        for (int i = 0; i < e.Count; i++)
        {
            Equal(e[i], a[i], $"item {i}");
        }
    }
    public static TException Throws<TException>(Action action) where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException ex)
        {
            return ex;
        }
        throw new AssertException($"expected {typeof(TException).Name} to be thrown");
    }

}

public static class TempFolder
{
    public static string Create()
    {
        
        string name = Guid.NewGuid().ToString("N");
        string path = Path.Combine(Path.GetTempPath(), "kilodb-tests", name);
        Directory.CreateDirectory(path);
        return path;
    }
}


