namespace KiloDb.Core;

public sealed class MemoryStore : IKeyValueStore
{
    public readonly SortedDictionary<string, string> _data = new(StringComparer.Ordinal);
    public string EngineName => "Engine1";

    public void Put(string key, string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(key);
        _data[key] = value;
    }

    public string? Get(string key) => _data.TryGetValue(key, out var value) ? value : null;
    public bool Delete(string key) => _data.Remove(key);

    public IEnumerable<KeyValuePair<string, string>> Scan(string? from = null, string? to = null)
    {
        foreach(var pair in _data)
        {
            if(from is not null && string.CompareOrdinal(pair.Key, from) < 0) continue;
            if(to is not null && string.CompareOrdinal(pair.Key, to) >= 0) yield break;
            yield return pair;
        }
    }

    public string GetStats() => $"engine=memory keys={_data.Count}";
    public void Dispose()
    {
        
    }
}