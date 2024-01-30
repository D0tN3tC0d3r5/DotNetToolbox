namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface IFlag : IArgument {
    Task<Result> Read(CancellationToken ct = default);
}
