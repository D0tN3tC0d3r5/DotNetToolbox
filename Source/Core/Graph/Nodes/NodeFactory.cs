namespace DotNetToolbox.Graph.Nodes;

internal sealed class NodeFactory(IGuidProvider? guid = null)
    : INodeFactory {
    public IConditionalNode If(Func<Context, bool> predicate, INode truePath, INode? falsePath = null)
        => ConditionalNode.Create(predicate, truePath, falsePath, guid);

    public IMappingNode<TKey> Select<TKey>(Func<Context, TKey> select, IReadOnlyDictionary<TKey, INode?> paths)
        where TKey : notnull
        => BranchingNode.Create(select, paths, guid);

    public IActionNode Do(Action<Context> action, INode? next = null, IPolicy? policy = null)
        => ActionNode.Create(action, next, policy, guid);

    public IActionNode Do<TAction>()
        where TAction : ActionNode<TAction>
        => ActionNode.Create<TAction>(guid);

    public INode Start
        => EntryNode.Create(guid);

    public INode Void
        => VoidNode.Instance;
}
