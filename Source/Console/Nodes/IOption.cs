namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface IOption : IArgument {
    Task<Result> Read(string? value, NodeContext context, CancellationToken ct = default);
}
