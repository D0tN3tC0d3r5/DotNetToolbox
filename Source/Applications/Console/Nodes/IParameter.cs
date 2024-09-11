namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface IParameter : IHasParent {
    int Order { get; }
    string? DefaultValue { get; }
    bool IsRequired { get; }
    bool IsSet { get; }

    Task<Result> Read(string? value, IMap context, CancellationToken ct = default);
}
