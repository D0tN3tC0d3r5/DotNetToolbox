namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

public abstract class Executable<TExecutable>
    : Node<TExecutable>, IExecutable
    where TExecutable : Executable<TExecutable> {

    protected Executable(IHasChildren node, string name, params string[] aliases)
        : base(node, name, aliases) {
    }

    public abstract Task<Result> ExecuteAsync(string[] args, CancellationToken ct = default);
}
