namespace DotNetToolbox.Graph.Nodes;

public interface IEntryNode
    : INode {
    INode? Next { get; set; }
}
