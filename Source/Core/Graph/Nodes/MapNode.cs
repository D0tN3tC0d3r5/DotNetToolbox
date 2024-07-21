namespace DotNetToolbox.Graph.Nodes;

public class MapNode
    : MapNode<string>,
      IMapNode {
    private MapNode(string? id, Func<Context, string> selectPath, IReadOnlyDictionary<string, INode?> options, IGuidProvider? guid = null)
        : base(id, selectPath, options, guid) {
    }

    public static MapNode Create(Func<Context, string> selectPath, IEnumerable<INode?> paths, IGuidProvider? guid = null)
        => new(id: null, selectPath, paths.Distinct().ToDictionary(k => k?.Id ?? string.Empty, v => v), guid);

    public static MapNode Create(string id, Func<Context, string> selectPath, IEnumerable<INode?> paths)
        => new(IsNotNullOrWhiteSpace(id), selectPath, paths.Distinct().ToDictionary(k => k?.Id ?? string.Empty, v => v));

    public static MapNode<TKey> Create<TKey>(Func<Context, TKey> selectPath, IReadOnlyDictionary<TKey, INode?> paths, IGuidProvider? guid = null)
        where TKey : notnull
        => new(id: null, selectPath, paths, guid);

    public static MapNode<TKey> Create<TKey>(string id, Func<Context, TKey> selectPath, IReadOnlyDictionary<TKey, INode?> paths)
        where TKey : notnull
        => new(IsNotNullOrWhiteSpace(id), selectPath, paths);

    public static TNode Create<TNode, TKey>(IGuidProvider? guid = null)
        where TKey : notnull
        where TNode : MapNode<TNode, TKey>
        => InstanceFactory.Create<TNode>(guid);

    public static TNode Create<TNode, TKey>(string id)
        where TKey : notnull
        where TNode : MapNode<TNode, TKey>
        => InstanceFactory.Create<TNode>(id);
}

public class MapNode<TKey>
    : MapNode<MapNode<TKey>, TKey>
    where TKey : notnull {
    internal MapNode(string? id,
                     Func<Context, TKey> selectPath,
                     IReadOnlyDictionary<TKey, INode?> options,
                     IGuidProvider? guid = null)
        : base(id, guid) {
        SelectPath = IsNotNull(selectPath);
        foreach (var (key, value) in options)
            SetOption(key, value);
    }

    protected sealed override Func<Context, TKey> SelectPath { get; }
}

public abstract class MapNode<TNode, TKey>(string? id, IGuidProvider? guid = null)
    : Node(id, guid),
      IMapNode<TKey>
    where TNode : MapNode<TNode, TKey>
    where TKey : notnull {
    private readonly Dictionary<TKey, INode?> _options = [];

    protected abstract Func<Context, TKey> SelectPath { get; }

    public void SetOption(TKey key, INode? path) {
        _options[key] = path;
        if (!Paths.Contains(path)) Paths.Add(path);
    }

    protected override Result IsValid() {
        var result = Success();
        if (Paths.Count == 0)
            result += Invalid($"No path is registered for node '{Id}'.");

        return result;
    }

    protected override INode GetNext(Context context)
        => _options.GetValueOrDefault(SelectPath(context))
        ?? throw new InvalidOperationException("The selected path was not found.");

    protected sealed override void UpdateState(Context context) { }
}
