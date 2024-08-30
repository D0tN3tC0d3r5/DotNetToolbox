namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface ICommand : IHasParent, IHasChildren {
    Task<Result> Execute(IReadOnlyList<string> args, CancellationToken ct = default);
}
