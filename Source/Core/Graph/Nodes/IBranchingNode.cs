namespace DotNetToolbox.Graph.Nodes;

public interface IBranchingNode
    : INode {
    Dictionary<string, INode?> Choices { get; }
}
