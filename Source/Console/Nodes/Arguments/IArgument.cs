namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public interface IArgument : INode, IHasParent {
    Task<Result> ClearData(CancellationToken ct);
    Task<Result> ReadData(string? value, CancellationToken ct);
}
