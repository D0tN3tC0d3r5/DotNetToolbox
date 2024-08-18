namespace DotNetToolbox.Graph.Nodes;

public interface IIfNode
    : INode {
    INode? Then { get; set; }
    INode? Else { get; set; }
}
