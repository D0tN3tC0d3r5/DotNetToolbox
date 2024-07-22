namespace DotNetToolbox.Graph.Nodes;

public interface IConditionalNode
    : INode {
    INode? True { get; set; }
    INode? False { get; set; }
}
