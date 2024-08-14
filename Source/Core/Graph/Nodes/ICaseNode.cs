namespace DotNetToolbox.Graph.Nodes;

public interface ICaseNode
    : INode {
    Dictionary<string, INode?> Choices { get; }
}
