namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public sealed class Option(IHasChildren parent, string name, params string[] aliases)
        : Option<Option>(parent, name, aliases) {
}

public abstract class Option<TOption>
    : Argument<TOption>
    , IOption
    where TOption : Option<TOption> {
    protected Option(IHasChildren parent, string name, params string[] aliases)
        : base(parent, name, aliases) {
    }

    public Task<Result> SetValue(string? value, CancellationToken ct) {
        Application.Data[Name] = value is "null" or "default"
            ? null
            : value;
        return OnDataRead(ct);
    }
}
