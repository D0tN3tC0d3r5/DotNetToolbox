namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

public sealed class Trigger : Trigger<Trigger> {
    internal Trigger(IHasChildren node, string name, string[] aliases, Func<Trigger, Result> execute)
        : base(node, name, aliases, execute) {
    }
}

public abstract class Trigger<TTrigger>
    : Executable<TTrigger>
    , ITrigger
    where TTrigger : Trigger<TTrigger> {
    private readonly Func<TTrigger, Result>? _execute;

    protected Trigger(IHasChildren parent, string name, params string[] aliases)
        : base(parent, name, aliases) {
    }

    protected Trigger(IHasChildren parent, string name, string[] aliases, Func<TTrigger, Result> execute)
        : this(parent, name, aliases) {
        _execute = IsNotNull(execute);
    }

    public sealed override Task<Result> ExecuteAsync(string[] _, CancellationToken ct = default)
        => Task.Run(() => _execute?.Invoke((TTrigger)this) ?? Execute(), ct);

    protected virtual Result Execute() => Success();
}
