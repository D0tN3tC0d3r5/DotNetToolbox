namespace DotNetToolbox.Data.Strategies.Key;

public sealed class InMemoryIntKeyHandler(string contextKey)
    : InMemoryKeyHandler<int>(contextKey, EqualityComparer<int>.Default) {
    protected override int GenerateNewKey(int lastUsedKey)
        => lastUsedKey + 1;
}
