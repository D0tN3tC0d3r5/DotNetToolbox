namespace DotNetToolbox.Graph.PathBuilder;

public static class Start {
    public static IThenBuilder If(Func<Map, bool> predicate)
        => PathBuilder.Start.If(predicate);
    public static IComparerBuilder<TKey> Select<TKey>(Func<Map, TKey> select)
        => PathBuilder.Start.When(select);
    public static IPathBuilder Do(Func<Map, INode> execute)
        => PathBuilder.Start.Do(execute);
}
