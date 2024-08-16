namespace DotNetToolbox.Graph.Nodes;

public interface IExitNode
    : INode {
    int ExitCode { get; }
}
