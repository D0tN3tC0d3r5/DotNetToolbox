namespace DotNetToolbox.Graph.Nodes;

public interface IEndNode
    : INode {
    int ExitCode { get; }
}
