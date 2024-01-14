namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public interface IFlag : IArgument {
    Task<Result> SetValue(CancellationToken ct);
}
