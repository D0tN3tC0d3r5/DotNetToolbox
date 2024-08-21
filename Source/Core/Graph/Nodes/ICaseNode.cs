namespace DotNetToolbox.Graph.Nodes;

public interface ICaseNode
    : INode {
    string Name { get; set; }
    Dictionary<string, INode?> Choices { get; }
}
