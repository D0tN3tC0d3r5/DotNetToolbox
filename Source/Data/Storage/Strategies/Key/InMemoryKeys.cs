namespace DotNetToolbox.Data.Strategies.Key;

public static class InMemoryKeys {
    private static readonly object _lock = new();
    private static readonly ConcurrentDictionary<string, KeyCollection> _repositoryKeys = new();

    internal static bool IsInUse<TKey>(string contextKey, TKey key) {
        lock (_lock) {
            var entry = _repositoryKeys.GetOrAdd(contextKey, _ => []);
            return key != null && entry.Contains(key);
        }
    }

    public static TKey GetNext<TKey>(string contextKey, TKey candidate, Func<TKey, TKey> generateKey) {
        lock (_lock) {
            var entry = _repositoryKeys.GetOrAdd(contextKey, _ => []);
            return candidate == null || Equals(candidate, default(TKey))
                ? GetOrAddNext(entry, generateKey)
                : entry.Contains(candidate)
                    ? throw new InvalidOperationException($"The key '{candidate}' is already in use in the repository.")
                    : AddKey(entry, candidate);
        }
    }

    private static TKey GetOrAddNext<TKey>(KeyCollection entry, Func<TKey, TKey> generateKey) {
        var last = entry.Last is null ? default! : (TKey)entry.Last;
        var result = generateKey(last);
        entry.Last = result;
        return result;
    }

    private static TKey AddKey<TKey>(KeyCollection entry, TKey candidate) {
        entry.Add(candidate!);
        entry.Last = candidate;
        return candidate;
    }
}
