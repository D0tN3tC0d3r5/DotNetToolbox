namespace DotNetToolbox.Graph.Nodes;

public interface IIfNode
    : INode {
    string Name { get; set; }
    INode? Then { get; set; }
    INode? Else { get; set; }
}
