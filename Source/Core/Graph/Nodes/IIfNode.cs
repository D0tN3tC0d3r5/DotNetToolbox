namespace DotNetToolbox.Graph.Nodes;

public interface IIfNode
    : INode {
    INode? TruePath { get; set; }
    INode? FalsePath { get; set; }
}
