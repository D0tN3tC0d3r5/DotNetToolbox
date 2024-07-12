namespace DotNetToolbox.Graph.PathBuilder;

public interface IElseBuilder
    : IPathTerminator {
    IPathBuilder Else(INode node);
    IPathBuilder Else(Func<IPathBuilder, INode> builder);
    IThenBuilder ElseIf(Func<Map, bool> predicate);
}
