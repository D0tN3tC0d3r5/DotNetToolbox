namespace DotNetToolbox.Graph.Utilities;

public abstract class NodeId<TKey>
    : NodeId {
    public abstract TKey Next { get; }
}

public abstract class NodeId {
    private static readonly ConcurrentDictionary<string, NodeId> _providers = [];
    public static SequentialNodeId FromSequential(string key, uint start = 1)
        => _providers.GetOrAdd(key, _ => new SequentialNodeId(start)) as SequentialNodeId
        ?? throw new InvalidOperationException($"The node id provider with key '{key}' is not a sequential provider.");

    public abstract void Reset();
}
