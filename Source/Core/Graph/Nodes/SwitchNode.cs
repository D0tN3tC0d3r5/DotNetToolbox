namespace DotNetToolbox.Graph.Nodes;

public class SwitchNode
    : SwitchNode<string> {
    public SwitchNode(string id, IReadOnlyDictionary<string, INode>? exits = null, Func<Map, string>? selectExit = null)
        : base(id, exits, selectExit) {
    }

    public SwitchNode(string id, IEnumerable<INode> exits, Func<Map, string>? selectExit = null)
        : base(id, exits.ToDictionary(GetKey, v => v).AsReadOnly(), selectExit) {
    }

    public void AddExit(INode node)
        => AddExit(GetKey(node), node);

    private static string GetKey(INode node) => $"{node.GetType().Name}_{node.Id}";
}

public class SwitchNode<TKey>
    : Node
    where TKey : notnull {
    private readonly Func<Map, TKey> _selectExit;
    private readonly Dictionary<TKey, INode> _exitMap = [];

    public SwitchNode(string id, IReadOnlyDictionary<TKey, INode>? exits = null, Func<Map, TKey>? selectExit = null)
        : base(id) {
        _selectExit = selectExit ?? Select;
        _exitMap = exits is null ? _exitMap : exits.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        Exits = [.. _exitMap.Values];
    }

    public void AddExit(TKey key, INode node) {
        if (!_exitMap.TryAdd(key, node))
            throw new InvalidOperationException($"The exit key '{key}' is already registered.");
        Exits.Add(node);
    }

    protected override Result IsValid() {
        var result = Success();
        if (Exits.Count == 0)
            result += Invalid($"No exit were registered for node '{Id}'.");
        return result;
    }

    protected override INode GetNext(Map state) {
        UpdateState(state);
        return _exitMap.GetValueOrDefault(_selectExit(state))
            ?? throw new InvalidOperationException("The selected exit was not registered.");
    }

    protected sealed override void UpdateState(Map state) => base.UpdateState(state);

    protected virtual TKey Select(Map state)
        => throw new NotImplementedException($"The node selection is not defined for node '{Id}'.");

}
