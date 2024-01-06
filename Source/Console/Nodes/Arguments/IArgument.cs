namespace ConsoleApplication.Nodes.Arguments;

public interface IArgument : ILeaf {
    Task<Result> ClearData(CancellationToken ct);
    Task<Result> ReadData(string? value, CancellationToken ct);
}
