namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public sealed class Option(IHasChildren parent, string name, params string[] aliases)
        : Option<Option>(parent, name, aliases);

public abstract class Option<TOption>(IHasChildren parent, string name, params string[] aliases)
    : Node<TOption>(parent, name, aliases), IOption
    where TOption : Option<TOption> {

    public sealed override Task<Result> ExecuteAsync(IReadOnlyList<string> args, CancellationToken ct = default) {
        Application.Data[Name] = args[0] is "null" or "default"
                                     ? null
                                     : args[0];
        return SuccessTask();
    }

    protected virtual Result Execute() => Success();
}
