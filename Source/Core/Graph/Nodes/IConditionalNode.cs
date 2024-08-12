namespace DotNetToolbox.Graph.Nodes;

public interface IConditionalNode
    : INode {
    INode? IsTrue { get; set; }
    INode? IsFalse { get; set; }
}
