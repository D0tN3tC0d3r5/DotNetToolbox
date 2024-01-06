namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

public interface IExecutable : IHasChildren {
    Task<Result> ExecuteAsync(string[] args, CancellationToken ct);
}
