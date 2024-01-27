namespace DotNetToolbox.ConsoleApplication.Nodes;

public sealed class Option(IHasChildren parent, string name, params string[] aliases)
        : Option<Option>(parent, name, aliases);

public abstract class Option<TOption>(IHasChildren parent, string name, params string[] aliases)
    : Node<TOption>(parent, name, aliases), IOption
    where TOption : Option<TOption> {

    public sealed override Task<Result> ExecuteAsync(IReadOnlyList<string> args, CancellationToken ct = default) {
        if (args.Count == 0) return InvalidTask($"Missing value for option '--{Name.ToLower()}'.");
        Application.Data[Name] = args[0] switch {
            "null" or "default" => null,
            ['"', ..var text, '"'] => text,
            _ => args[0],
        };
        return SuccessTask();
    }
}
