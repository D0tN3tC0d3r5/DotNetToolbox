namespace DotNetToolbox.Graph.PathBuilder;

public interface IThenBuilder
    : IPathBuilder {
    IPathBuilder Else(INode node);
    IPathBuilder Else(Action<IPathBuilder> builder);
    IIfBuilder ElseIf(Func<Map, bool> predicate);
}
