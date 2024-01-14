namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public interface IOption : IArgument {
    Task<Result> SetValue(string value, CancellationToken ct);
}
