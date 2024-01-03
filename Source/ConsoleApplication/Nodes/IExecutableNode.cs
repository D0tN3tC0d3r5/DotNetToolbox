namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface IExecutableNode : INode {
    Task<Result> ExecuteAsync(string[] input, CancellationToken ct);
}
