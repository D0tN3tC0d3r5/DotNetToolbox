namespace DotNetToolbox.Graph.PathBuilder;

public interface IThenBuilder
    : IPathTerminator {
    IElseBuilder Then(INode node);
    IElseBuilder Then(Func<IPathBuilder, INode> builder);
}
