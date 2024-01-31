namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface IParameter : IHasParent {
    bool IsSet { get; }
    bool IsRequired { get; }
    int Order { get; }

    Task<Result> Read(string? value, NodeContext context, CancellationToken ct = default);
}
