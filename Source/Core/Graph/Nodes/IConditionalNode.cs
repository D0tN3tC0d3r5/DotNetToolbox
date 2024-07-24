namespace DotNetToolbox.Graph.Nodes;

public interface IConditionalNode
    : INode {
    INode? IsTrue { get; }
    INode? IsFalse { get; }
}
