namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

public interface IExecutable : INode {
    Task<Result> ExecuteAsync(string[] args, CancellationToken ct = default);
}
