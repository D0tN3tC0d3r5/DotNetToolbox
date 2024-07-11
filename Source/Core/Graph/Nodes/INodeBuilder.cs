namespace DotNetToolbox.Graph.Nodes;

public interface INodeBuilder
    : IPathBuilder {
    INode Build();
}
