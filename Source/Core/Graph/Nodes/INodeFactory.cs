namespace DotNetToolbox.Graph.Nodes;

public interface INodeFactory {
    IConditionalNode If(Func<Context, bool> predicate,
                        INode truePath,
                        INode? falsePath = null);

    IMappingNode<TKey> Select<TKey>(Func<Context, TKey> select,
                                    IReadOnlyDictionary<TKey, INode?> paths)
        where TKey : notnull;

    IActionNode Do(Action<Context> action,
                   INode? next = null,
                   IPolicy? policy = null);

    IActionNode Do<TAction>()
        where TAction : ActionNode<TAction>;

    INode Start { get; }

    INode Void { get; }
}
