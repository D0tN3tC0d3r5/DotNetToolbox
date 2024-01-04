namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface IExecutable : INode {
    Task<Result> ExecuteAsync(string[] input, CancellationToken ct);
}
