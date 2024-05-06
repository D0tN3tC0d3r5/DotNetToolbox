namespace DotNetToolbox.Data.Strategies.Key;

public sealed class NullKeyHandler<TKey>
    : KeyHandler<TKey> {
    public static NullKeyHandler<TKey> Default => new();
    private NullKeyHandler()
        : base(EqualityComparer<TKey>.Default) {
    }

    public override bool IsInUse(TKey key) => false;
    public override TKey GetNext(TKey candidate) => candidate;
    public override Task<bool> IsInUseAsync(TKey key, CancellationToken ct = default)
        => Task.FromResult(IsInUse(key));
    public override Task<TKey> GetNextAsync(TKey candidate, CancellationToken ct = default)
        => Task.FromResult(GetNext(candidate));
}
