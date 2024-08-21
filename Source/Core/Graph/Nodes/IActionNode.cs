namespace DotNetToolbox.Graph.Nodes;

public interface IActionNode : INode {
    string Name { get; set; }
    IRetryPolicy Retry { get; set; }
}
