namespace DotNetToolbox.Graph.Nodes;

public interface IActionNode : INode {
    IRetryPolicy Retry { get; set; }
}
