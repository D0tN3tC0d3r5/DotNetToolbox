namespace DotNetToolbox.Graph.Nodes;

public interface INodeFactory {
    INode If(Func<Context, bool> predicate, INode truePath, INode? falsePath = null);

    INode Switch<TKey>(Func<Context, TKey> select, IReadOnlyDictionary<TKey, INode?> paths)
        where TKey : notnull;

    INode Switch(Func<Context, string> select, IReadOnlyDictionary<string, INode?> paths);

    INode Switch(Func<Context, string> select, IEnumerable<INode?> paths);

    INode Do(Action<Context> action, INode? node);

    INode Void { get; }
}
