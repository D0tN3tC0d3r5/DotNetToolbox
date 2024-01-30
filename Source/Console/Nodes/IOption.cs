namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface IOption : IArgument {
    Task<Result> Read(string? value, CancellationToken ct = default);
}
