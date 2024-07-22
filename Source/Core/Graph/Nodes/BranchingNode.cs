namespace DotNetToolbox.Graph.Nodes;

public class BranchingNode
    : BranchingNode<string>,
      IMappingNode {
    private BranchingNode(Func<Context, string> selectPath,
                          IReadOnlyDictionary<string, INode?> options,
                          IGuidProvider? guid = null)
        : base(selectPath, options, guid) {
    }

    public static BranchingNode Create(Func<Context, string> selectPath, IEnumerable<INode?> paths, IGuidProvider? guid = null)
        => new(selectPath, paths.Distinct().ToDictionary(k => k?.Id ?? string.Empty, v => v), guid);

    public static BranchingNode<TKey> Create<TKey>(Func<Context, TKey> selectPath, IReadOnlyDictionary<TKey, INode?> paths, IGuidProvider? guid = null)
        where TKey : notnull
        => new(selectPath, paths, guid);

    public static TNode Create<TNode, TKey>(IGuidProvider? guid = null)
        where TKey : notnull
        where TNode : BranchingNode<TNode, TKey>
        => InstanceFactory.Create<TNode>(guid);
}

public class BranchingNode<TKey>
    : BranchingNode<BranchingNode<TKey>, TKey>
    where TKey : notnull {
    internal BranchingNode(Func<Context, TKey> selectPath,
                           IReadOnlyDictionary<TKey, INode?> options,
                           IGuidProvider? guid = null)
        : base(guid) {
        SelectPath = IsNotNull(selectPath);
        foreach (var (key, value) in options)
            SetOption(key, value);
    }

    protected sealed override Func<Context, TKey> SelectPath { get; }
}

public abstract class BranchingNode<TNode, TKey>(IGuidProvider? guid = null)
    : Node(guid),
      IMappingNode<TKey>
    where TNode : BranchingNode<TNode, TKey>
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
