namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface IFlag : IArgument {
    Task<Result> Read(NodeContext context, CancellationToken ct = default);
}
