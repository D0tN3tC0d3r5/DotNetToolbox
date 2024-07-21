namespace DotNetToolbox.Graph.Nodes;

internal sealed class NodeFactory
    : INodeFactory {
    private static readonly INode _void = new VoidNode();

    public IIfNode If(Func<Context, bool> predicate, INode truePath, INode? falsePath = null, IGuidProvider? guid = null)
        => IfNode.Create(predicate, truePath, falsePath, guid);

    public IIfNode If(string id, Func<Context, bool> predicate, INode truePath, INode? falsePath = null)
        => IfNode.Create(id, predicate, truePath, falsePath);

    public IMapNode<TKey> Select<TKey>(Func<Context, TKey> select, IReadOnlyDictionary<TKey, INode?> paths, IGuidProvider? guid = null)
        where TKey : notnull
        => MapNode.Create(select, paths, guid);

    public IMapNode<TKey> Select<TKey>(string id, Func<Context, TKey> select, IReadOnlyDictionary<TKey, INode?> paths)
        where TKey : notnull
        => MapNode.Create(id, select, paths);

    public IMapNode Select(Func<Context, string> select, IEnumerable<INode?> paths, IGuidProvider? guid = null)
        => MapNode.Create(select, paths, guid);

    public IMapNode Select(string id, Func<Context, string> select, IEnumerable<INode?> paths)
        => MapNode.Create(id, select, paths);

    public IActionNode Do(Action<Context> action, INode? next = null, IPolicy? policy = null, IGuidProvider? guid = null)
        => ActionNode.Create(action, next, policy, guid);

    public IActionNode Do(string id, Action<Context> action, INode? next = null, IPolicy? policy = null)
        => ActionNode.Create(id, action, next, policy);

    public IActionNode Do<TAction>(IGuidProvider? guid = null)
        where TAction : ActionNode<TAction>
        => ActionNode.Create<TAction>(guid);

    public IActionNode Do<TAction>(string id)
        where TAction : ActionNode<TAction>
        => ActionNode.Create<TAction>(id);

    public INode Void => _void;
}
