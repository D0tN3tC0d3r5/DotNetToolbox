namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public interface IHasValue {
    Task<Result> SetValue(string input, CancellationToken ct);
}
