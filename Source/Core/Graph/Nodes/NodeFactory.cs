namespace DotNetToolbox.Graph.Nodes;

internal sealed class NodeFactory
    : INodeFactory {
    private static readonly INode _void = new VoidNode();

    public INode If(Func<Context, bool> predicate, INode truePath, INode? falsePath = null)
        => IfNode.Create(predicate, truePath, falsePath);

    public INode Select<TKey>(Func<Context, TKey> select, IReadOnlyDictionary<TKey, INode?> paths)
        where TKey : notnull
        => SelectNode.Create(paths, select);

    public INode Select(Func<Context, string> select, IEnumerable<INode?> paths)
        => SelectNode.Create(paths, select);

    public INode Do(Action<Context> action, INode? next = null)
        => ActionNode.Create(action, next);

    public INode Void => _void;
}
