namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface IOption : IArgument {
    Task<Result> Read(string? value, IContext context, CancellationToken ct = default);
}
