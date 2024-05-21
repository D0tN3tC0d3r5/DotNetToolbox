namespace DotNetToolbox.Data.Strategies.Key {
    public static class InMemoryKeyHandlerFactory {
        public static IKeyHandler<TKey> CreateDefault<TKey>(string contextKey) {
            if (typeof(TKey) == typeof(Guid)) return (IKeyHandler<TKey>)(object)new InMemoryGuidKeyHandler(contextKey);
            if (typeof(TKey) == typeof(int)) return (IKeyHandler<TKey>)(object)new InMemoryIntKeyHandler(contextKey);
            if (typeof(TKey) == typeof(long)) return (IKeyHandler<TKey>)(object)new InMemoryLongKeyHandler(contextKey);
            if (typeof(TKey) == typeof(string)) return (IKeyHandler<TKey>)(object)new InMemoryStringKeyHandler(contextKey);
            if (typeof(TKey) == typeof(DateTimeOffset)) return (IKeyHandler<TKey>)(object)new InMemoryDateTimeKeyHandler(contextKey);
            return NullKeyHandler<TKey>.Default;
        }
    }
}
