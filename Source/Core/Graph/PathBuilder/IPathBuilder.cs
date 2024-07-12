namespace DotNetToolbox.Graph.PathBuilder;

public interface IPathBuilder
    : IPathTerminator {
    IThenBuilder If(Func<Map, bool> predicate);
    IComparerBuilder<TKey> When<TKey>(Func<Map, TKey> select);
    IPathBuilder Do(Func<Map, INode> execute);
}
