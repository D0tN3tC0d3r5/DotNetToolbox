namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface IFlag : IArgument {
    Task<Result> Read(IContext context, CancellationToken ct = default);
}
