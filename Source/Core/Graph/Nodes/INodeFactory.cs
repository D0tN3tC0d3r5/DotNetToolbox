namespace DotNetToolbox.Graph.Nodes;

public interface INodeFactory {
    INode If(Func<Context, bool> predicate, INode truePath, INode? falsePath = null);

    INode Select<TKey>(Func<Context, TKey> select, IReadOnlyDictionary<TKey, INode?> paths)
        where TKey : notnull;

    INode Select(Func<Context, string> select, IEnumerable<INode?> paths);

    INode Do(Action<Context> action, INode? node);

    INode Void { get; }
}
