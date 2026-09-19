namespace KiloDb.Core;

public interface IKeyValueStore : IDisposable
{
    string EngineName {get;}
    void Put (string key, string value);
    string? Get(string key);

    bool Delete(string key);
    IEnumerable<KeyValuePair<string, string>> Scan(string? from = null, string? to = null);
    string GetStats();
}