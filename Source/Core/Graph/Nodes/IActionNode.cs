namespace DotNetToolbox.Graph.Nodes;

public interface IActionNode
    : INode {
    INode? Next { get; }
}
