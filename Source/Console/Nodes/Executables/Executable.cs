namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

public abstract class Executable<TExecutable>
    : Node<TExecutable>, IExecutable
    where TExecutable : Executable<TExecutable> {

    protected Executable(IHasChildren node, string name, params string[] aliases)
        : base(node, name, aliases) {
    }

    public ICollection<INode> Children { get; } = [];

    public async Task<Result> ExecuteAsync(string[] args, CancellationToken ct) {
        var result = await InputReader.ParseTokens([.. Children], args, ct);
        return result.IsSuccess
                   ? await ExecuteAsync(ct)
                   : result;
    }

    protected abstract Task<Result> ExecuteAsync(CancellationToken ct);
}
