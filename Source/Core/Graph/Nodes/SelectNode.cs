namespace DotNetToolbox.Graph.Nodes;

public class SelectNode
    : SelectNode<string>
    , ISelectNode {
    public static SelectNode Create(IEnumerable<INode?> paths, Func<Context, string> selectPath, IGuidProvider? guid = null)
        => new(null, paths.Distinct().ToDictionary(k => k?.Id ?? string.Empty, v => v), selectPath, guid);

    public static SelectNode<TKey> Create<TKey>(IReadOnlyDictionary<TKey, INode?> paths, Func<Context, TKey> selectPath, IGuidProvider? guid = null)
        where TKey : notnull
        => new(null, paths, selectPath, guid);

    public static SelectNode Create(string id, IEnumerable<INode?> paths, Func<Context, string> selectPath)
        => new(IsNotNullOrWhiteSpace(id), paths.Distinct().ToDictionary(k => k?.Id ?? string.Empty, v => v), selectPath);

    public static SelectNode<TKey> Create<TKey>(string id, IReadOnlyDictionary<TKey, INode?> paths, Func<Context, TKey> selectPath)
    where TKey : notnull
        => new(IsNotNullOrWhiteSpace(id), paths, selectPath);

    private SelectNode(string? id, IReadOnlyDictionary<string, INode?> options, Func<Context, string> selectPath, IGuidProvider? guid = null)
        : base(id, options, selectPath, guid) {
    }
}

public class SelectNode<TKey>
    : Node
    , ISelectNode<TKey>
    where TKey : notnull {
    private readonly Func<Context, TKey> _selectPath;
    private readonly Dictionary<TKey, INode?> _options;

    internal SelectNode(string? id, IReadOnlyDictionary<TKey, INode?> options, Func<Context, TKey> selectPath, IGuidProvider? guid = null)
        : base(id, guid) {
        _selectPath = IsNotNull(selectPath);
        _options = IsNotNull(options).ToDictionary();
        Paths = [.. _options.Values.Distinct()];
    }

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

    protected override INode GetNext(Context state) {
        UpdateState(state);
        return _options.GetValueOrDefault(_selectPath(state))
            ?? throw new InvalidOperationException("The selected path was not found.");
    }

    protected sealed override void UpdateState(Context state) { }
}
