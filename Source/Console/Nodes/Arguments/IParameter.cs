namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public interface IParameter : IArgument {
    bool IsSet { get; }
    bool IsRequired { get; }
    int Order { get; }
    Task<Result> SetValue(string value, CancellationToken ct);
}
