namespace DotNetToolbox.Graph.Utilities;

public abstract class IdGenerator<TGenerator, TKey>
    : IIdGenerator<TKey>
    where TGenerator : IdGenerator<TGenerator, TKey> {
    private static readonly ConcurrentDictionary<string, TGenerator> _providers = [];

    public static TGenerator Shared
        => _providers.GetOrAdd(string.Empty, _ => InstanceFactory.Create<TGenerator>());

    public static TGenerator Keyed(string key)
        => _providers.GetOrAdd(IsNotNullOrWhiteSpace(key), _ => InstanceFactory.Create<TGenerator>());

    public abstract TKey First { get; }
    public virtual TKey Next => ConsumeNext();
    public abstract TKey PeekNext();
    public abstract TKey ConsumeNext();
    public abstract void SetNext(TKey next);
    public abstract void Reset(TKey? first = default);
}
