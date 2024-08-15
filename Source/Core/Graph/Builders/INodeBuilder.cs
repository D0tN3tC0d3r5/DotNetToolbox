namespace DotNetToolbox.Graph.Builders;

public interface INodeBuilder<out TNode>
    where TNode : INode {
    TNode? Build();
}
