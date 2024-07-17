namespace DotNetToolbox.Graph.Nodes;

internal sealed class NodeFactory
    : INodeFactory {
    private static readonly INode _void = new VoidNode();

    public IIfNode If(Func<Context, bool> predicate, INode truePath, INode? falsePath = null, IGuidProvider? guid = null)
        => IfNode.Create(predicate, truePath, falsePath, guid);

    public IIfNode If(string id, Func<Context, bool> predicate, INode truePath, INode? falsePath = null)
        => IfNode.Create(id, predicate, truePath, falsePath);

    public ISelectNode<TKey> Select<TKey>(Func<Context, TKey> select, IReadOnlyDictionary<TKey, INode?> paths, IGuidProvider? guid = null)
        where TKey : notnull
        => SelectNode.Create(paths, select, guid);

    public ISelectNode<TKey> Select<TKey>(string id, Func<Context, TKey> select, IReadOnlyDictionary<TKey, INode?> paths)
        where TKey : notnull
        => SelectNode.Create(id, paths, select);

    public ISelectNode Select(Func<Context, string> select, IEnumerable<INode?> paths, IGuidProvider? guid = null)
        => SelectNode.Create(paths, select, guid);

    public ISelectNode Select(string id, Func<Context, string> select, IEnumerable<INode?> paths)
        => SelectNode.Create(id, paths, select);

    public IActionNode Do(Action<Context> action, INode? next = null, IGuidProvider? guid = null)
        => ActionNode.Create(action, next, guid);

    public IActionNode Do(string id, Action<Context> action, INode? next = null)
        => ActionNode.Create(id, action, next);

    public INode Void => _void;
}
