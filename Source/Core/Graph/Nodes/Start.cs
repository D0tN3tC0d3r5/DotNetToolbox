namespace DotNetToolbox.Graph.Nodes;

public static class Start {
    public static INode? If(Func<Context, bool> predicate, INode truePath, INode? falsePath = null)
        => NodeBuilder.Start.If(predicate, truePath, falsePath);

    public static INode? Select<TKey>(Func<Context, TKey> select, Dictionary<TKey, INode> paths)
        => NodeBuilder.Start.Switch(select, paths);

    public static INode? Do(Func<Context, INode> action)
        => NodeBuilder.Start.Do(action);

    public static INode? Void
        => new VoidNode();
}
