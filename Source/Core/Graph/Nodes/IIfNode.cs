namespace DotNetToolbox.Graph.Nodes;

public interface IIfNode
    : INode {
    INode? IsTrue { get; set; }
    INode? IsFalse { get; set; }
}
