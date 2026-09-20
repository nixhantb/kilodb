using KiloDb.Core;

namespace KiloDb.Tests;

public static class Step01MemoryStoreTests
{
    [Test]
    public static void PutThenGetReturnsValue()
    {
        using var store = new MemoryStore();
        store.Put("a", "1");
        Assert.Equal("1", store.Get("a"));
    }

    [Test]
    public static void MissingKeyReturnsNull()
    {
        using var store = new MemoryStore();
        Assert.Null(store.Get("nope"));
    }

    [Test]
    public static void ScanIsSortedAndRespectsRange()
    {
        using var store = new MemoryStore();
        foreach (var k in new[] { "d", "a", "c", "b" })
        {
            store.Put(k, k.ToUpper());
        }

        var keys = store.Scan("b", "d").Select(p => p.Key);
        Assert.SequenceEqual(["b", "c"], keys);
    }
    [Test]
    public static void CommandProcessorHandlesValuesWithSpaces()
    {
        using var store = new MemoryStore();
        var shell = new CommandProcessor(store);
        Assert.Equal("OK", shell.Execute("set greeting hello big world"));
        Assert.Equal("\"hello big world\"", shell.Execute("get greeting"));
        Assert.True(shell.Execute("bogus").StartsWith("ERR"));
    }
}