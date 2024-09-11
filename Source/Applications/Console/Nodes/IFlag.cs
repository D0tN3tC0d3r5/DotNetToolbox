namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface IFlag : IArgument {
    Task<Result> Read(IMap context, CancellationToken ct = default);
}
