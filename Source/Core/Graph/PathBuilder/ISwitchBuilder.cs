namespace DotNetToolbox.Graph.PathBuilder;

public interface ISwitchBuilder<in TKey>
    : IPathBuilder {
    IPathBuilder AddPath(TKey key, INode node);
    IPathBuilder AddPath(TKey key, Action<IPathBuilder> builder);
}
