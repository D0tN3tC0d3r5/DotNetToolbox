namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

public sealed class AsyncTrigger : AsyncTrigger<AsyncTrigger> {
    internal AsyncTrigger(IHasChildren node, string name, string[] aliases, Func<AsyncTrigger, CancellationToken, Task<Result>> execute)
        : base(node, name, aliases, execute) {
    }
}

public abstract class AsyncTrigger<TTrigger>
    : Executable<TTrigger>
    , IAsyncTrigger
    where TTrigger : AsyncTrigger<TTrigger> {
    private readonly Func<TTrigger, CancellationToken, Task<Result>>? _execute;

    protected AsyncTrigger(IHasChildren parent, string name, params string[] aliases)
        : base(parent, name, aliases) {
    }

    protected AsyncTrigger(IHasChildren parent, string name, string[] aliases, Func<TTrigger, CancellationToken, Task<Result>> execute)
        : this(parent, name, aliases) {
        _execute = IsNotNull(execute);
    }

    public sealed override Task<Result> ExecuteAsync(string[] _, CancellationToken ct = default)
        => _execute?.Invoke((TTrigger)this, ct) ?? ExecuteAsync(ct);

    protected virtual Task<Result> ExecuteAsync(CancellationToken ct) => SuccessTask();
}
