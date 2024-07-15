namespace DotNetToolbox.Graph.Nodes;

public static class Start {
    public static INode If(Func<Context, bool> predicate, INode truePath, INode? falsePath = null)
        => NodeBuilder.Start.If(predicate, truePath, falsePath);

    public static INode Select<TKey>(Func<Context, TKey> select, IReadOnlyDictionary<TKey, INode?> paths)
        where TKey : notnull
        => NodeBuilder.Start.Switch(select, paths);

    public static INode Select(Func<Context, string> select, IEnumerable<INode?> paths)
        => NodeBuilder.Start.Switch(select, paths);

    public static INode Do(Action<Context> action, INode? next)
        => NodeBuilder.Start.Do(action, next);

    public static INode Void
        => new VoidNode();
}
