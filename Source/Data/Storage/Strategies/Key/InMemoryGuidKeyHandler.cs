namespace DotNetToolbox.Data.Strategies.Key;

public sealed class InMemoryGuidKeyHandler(string contextKey, IGuidProvider? guid = null) 
    : InMemoryKeyHandler<Guid>(contextKey, EqualityComparer<Guid>.Default) {
    private readonly IGuidProvider _guid = guid ?? GuidProvider.Default;

    protected override Guid GenerateNewKey(Guid lastUsedKey)
        => _guid.Create();
}
