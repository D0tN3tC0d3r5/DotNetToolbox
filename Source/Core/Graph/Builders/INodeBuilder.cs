namespace DotNetToolbox.Graph.Builders;

public interface INodeBuilder {
    Result<INode?> Build();
}
