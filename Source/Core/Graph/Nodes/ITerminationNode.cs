namespace DotNetToolbox.Graph.Nodes;

public interface ITerminationNode
    : INode {
    int ExitCode { get; }
}
