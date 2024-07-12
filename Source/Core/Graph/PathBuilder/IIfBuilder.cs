namespace DotNetToolbox.Graph.PathBuilder;

public interface IIfBuilder {
    IThenBuilder Then(INode node);
    IThenBuilder Then(Action<IPathBuilder> builder);
}
