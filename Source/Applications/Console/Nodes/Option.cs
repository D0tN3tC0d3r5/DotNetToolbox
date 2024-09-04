namespace DotNetToolbox.ConsoleApplication.Nodes;

public sealed class Option(IHasChildren parent, string name, params string[] aliases)
        : Option<Option>(parent, name, aliases);

public abstract class Option<TOption>(IHasChildren parent, string name, params string[] aliases)
    : Node<TOption>(parent, name, aliases), IOption
    where TOption : Option<TOption> {
    Task<Result> IOption.Read(string? value, IContext context, CancellationToken ct) {
        context[Name] = value switch {
            null or "null" or "default" => null!,
            ['"', .. var text, '"'] => text,
            _ => value,
        };

        return Execute(ct);
    }

    protected virtual Task<Result> Execute(CancellationToken ct = default) => SuccessTask();
}
