namespace DotNetToolbox.Data.Strategies.Key;

public sealed class InMemoryLongKeyHandler(string contextKey)
    : InMemoryKeyHandler<long>(contextKey, EqualityComparer<long>.Default) {
    protected override long GenerateNewKey(long lastUsedKey)
        => lastUsedKey + 1;
}
