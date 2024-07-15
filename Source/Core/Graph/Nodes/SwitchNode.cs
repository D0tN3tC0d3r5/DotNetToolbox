namespace DotNetToolbox.Graph.Nodes;

public class SwitchNode
    : SwitchNode<string> {
    public static SwitchNode Create(IReadOnlyDictionary<string, INode> paths, Func<Context, string> selectPath)
        => new(Guid.NewGuid().ToString(), paths, selectPath);

    public static SwitchNode Create(IEnumerable<INode> paths, Func<Context, string> selectPath)
        => new(Guid.NewGuid().ToString(), paths, selectPath);

    public SwitchNode(string id, IReadOnlyDictionary<string, INode> paths, Func<Context, string> selectPath)
        : base(id, paths, selectPath) {
    }

    public SwitchNode(string id, IEnumerable<INode> paths, Func<Context, string> selectPath)
        : base(id, paths.ToDictionary(GetKey, static v => v).AsReadOnly(), selectPath) {
    }

    private static string GetKey(INode node)
        => $"{node.GetType().Name}_{node.Id}";
}

public class SwitchNode<TKey>
    : Node
    where TKey : notnull {
    private readonly Func<Context, TKey> _selectPath;
    private readonly IReadOnlyDictionary<TKey, INode> _pathMap;

    public static SwitchNode<T> Create<T>(IReadOnlyDictionary<T, INode> paths, Func<Context, T> selectPath)
        where T : notnull
        => new(Guid.NewGuid().ToString(), paths, selectPath);

    protected SwitchNode(string id, IReadOnlyDictionary<TKey, INode> paths, Func<Context, TKey> selectPath)
        : base(id) {
        _selectPath = IsNotNull(selectPath);
        _pathMap = IsNotNull(paths).ToDictionary();
        Paths = [.. _pathMap.Values];
    }

    protected override Result IsValid() {
        var result = Success();
        if (Paths.Count == 0)
            result += Invalid($"No path were registered for node '{Id}'.");
        return result;
    }

    protected override INode GetNext(Context state) {
        UpdateState(state);
        return _pathMap.GetValueOrDefault(_selectPath(state))
            ?? throw new InvalidOperationException("The selected path was not found.");
    }

    protected sealed override void UpdateState(Context state)
        => base.UpdateState(state);

    protected virtual TKey Select(Context state)
        => throw new NotImplementedException($"The node selection is not defined for node '{Id}'.");
}
