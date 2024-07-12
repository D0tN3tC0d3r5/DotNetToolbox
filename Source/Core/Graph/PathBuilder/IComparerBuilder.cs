namespace DotNetToolbox.Graph.PathBuilder;

public interface IComparerBuilder<in TKey>
    : IPathBuilder {
    IPathBuilder Is(TKey key);
}
