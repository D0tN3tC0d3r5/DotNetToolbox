namespace DotNetToolbox.Graph.Nodes;

public static class SelectNode {
    public static SelectNode<string> Create(IEnumerable<INode?> paths, Func<Context, string> selectPath, IGuidProvider? guid = null)
        => new(null, paths.Distinct().ToDictionary(k => k?.Id ?? string.Empty, v => v), selectPath, guid);

    public static SelectNode<TKey> Create<TKey>(IReadOnlyDictionary<TKey, INode?> paths, Func<Context, TKey> selectPath, IGuidProvider? guid = null)
        where TKey : notnull
        => new(null, paths, selectPath, guid);

    public static SelectNode<string> Create(string id, IEnumerable<INode?> paths, Func<Context, string> selectPath)
        => new(IsNotNullOrWhiteSpace(id), paths.Distinct().ToDictionary(k => k?.Id ?? string.Empty, v => v), selectPath);

    public static SelectNode<TKey> Create<TKey>(string id, IReadOnlyDictionary<TKey, INode?> paths, Func<Context, TKey> selectPath)
    where TKey : notnull
        => new(IsNotNullOrWhiteSpace(id), paths, selectPath);

}

public class SelectNode<TKey>
    : Node
    where TKey : notnull {
    private readonly Func<Context, TKey> _selectPath;
    private readonly IReadOnlyDictionary<TKey, INode?> _pathMap;

    internal SelectNode(string? id, IReadOnlyDictionary<TKey, INode?> paths, Func<Context, TKey> selectPath, IGuidProvider? guid = null)
        : base(id, guid) {
        _selectPath = IsNotNull(selectPath);
        _pathMap = IsNotNull(paths).ToDictionary();
        Paths = [.. _pathMap.Values];
    }

    protected override Result IsValid() {
        var result = Success();
        if (Paths.Count == 0)
            result += Invalid($"No path is registered for node '{Id}'.");
        return result;
    }

    protected override INode GetNext(Context state) {
        UpdateState(state);
        return _pathMap.GetValueOrDefault(_selectPath(state))
            ?? throw new InvalidOperationException("The selected path was not found.");
    }

    protected sealed override void UpdateState(Context state)
        => base.UpdateState(state);
}
