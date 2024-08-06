namespace DotNetToolbox.Data.Strategies.Key;

public static class InMemoryKeyHandlerFactory {
    public static IKeyHandler<TKey> CreateDefault<TKey>(string contextKey)
        => typeof(TKey).Name switch {
            nameof(Guid) => (IKeyHandler<TKey>)(object)new InMemoryGuidKeyHandler(contextKey),
            nameof(Int32) => (IKeyHandler<TKey>)(object)new InMemoryIntKeyHandler(contextKey),
            nameof(Int64) => (IKeyHandler<TKey>)(object)new InMemoryLongKeyHandler(contextKey),
            nameof(String) => (IKeyHandler<TKey>)(object)new InMemoryStringKeyHandler(contextKey),
            nameof(DateTimeOffset) => (IKeyHandler<TKey>)(object)new InMemoryDateTimeKeyHandler(contextKey),
            _ => NullKeyHandler<TKey>.Default
        };
}
