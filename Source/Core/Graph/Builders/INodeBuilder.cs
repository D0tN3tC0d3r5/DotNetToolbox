namespace DotNetToolbox.Graph.Builders;

public interface INodeBuilder<TNode>
    where TNode : INode {
    TNode Build();
}
