namespace DotNetToolbox.Graph.Nodes;

public static class Start {
    public static INode If(Func<Context, bool> predicate, INode truePath, INode? falsePath = null)
        => NodeFactory.Start.If(predicate, truePath, falsePath);

    public static INode Select<TKey>(Func<Context, TKey> select, IReadOnlyDictionary<TKey, INode?> paths)
        where TKey : notnull
        => NodeFactory.Start.Switch(select, paths);

    public static INode Select(Func<Context, string> select, IEnumerable<INode?> paths)
        => NodeFactory.Start.Switch(select, paths);

    public static INode Do(Action<Context> action, INode? next)
        => NodeFactory.Start.Do(action, next);

    public static INode Void
        => new VoidNode();
}
