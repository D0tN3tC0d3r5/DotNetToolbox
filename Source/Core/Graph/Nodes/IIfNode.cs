namespace DotNetToolbox.Graph.Nodes;

public interface IIfNode
    : INode {
    INode? True { get; set; }
    INode? False { get; set; }
}
