namespace DotNetToolbox.Graph.Nodes;

public interface IJumpNode
    : INode {
    string TargetTag { get; }
}
