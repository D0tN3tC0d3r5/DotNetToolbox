namespace DotNetToolbox.Data.Strategies.Key;

public sealed class InMemoryStringKeyHandler(string contextKey)
    : InMemoryKeyHandler<string>(contextKey, EqualityComparer<string>.Default) {
    protected override string GenerateNewKey(string lastUsedKey)
        => default!;
}
