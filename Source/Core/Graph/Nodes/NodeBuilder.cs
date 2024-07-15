namespace DotNetToolbox.Graph.Nodes;

internal sealed class NodeBuilder
    : INodeBuilder {
    private static readonly INode _void = new VoidNode();

    public static INodeBuilder Start => new NodeBuilder();

    public INode If(Func<Context, bool> predicate, INode truePath, INode? falsePath = null)
        => IfNode.Create(predicate, truePath, falsePath);

    public INode Switch<TKey>(Func<Context, TKey> select, IReadOnlyDictionary<TKey, INode?> paths)
        where TKey : notnull
        => SelectNode.Create(paths, select);

    public INode Switch(Func<Context, string> select, IReadOnlyDictionary<string, INode?> paths)
        => SelectNode.Create(paths, select);

    public INode Switch(Func<Context, string> select, IEnumerable<INode?> paths)
        => SelectNode.Create(paths, select);

    public INode Do(Action<Context> action, INode? next)
        => throw new NotImplementedException();

    public INode Void => _void;
}
