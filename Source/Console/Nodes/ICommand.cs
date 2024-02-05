namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface ICommand : IHasParent, IHasChildren {
    Task<Result> Set(IReadOnlyList<string> args, CancellationToken ct = default);

    Task<Result> Execute(CancellationToken ct = default);
}
