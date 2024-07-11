namespace DotNetToolbox.Graph;

public interface IGraphBuilder {
    IGraph Build();
    IGraphBuilder AddNode(INode node);
    IGraphBuilder AddEdgeTo(INode from, INode to);
    IGraphBuilder AddEndTo(INode from, string id = "-1");
}
