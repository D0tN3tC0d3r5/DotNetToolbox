namespace ConsoleApplication.Nodes.Arguments;

public sealed class Option
    : Option<Option> {
    public Option(IHasChildren parent, string name, params string[] aliases)
        : base(parent, name, aliases) {
    }
}

public abstract class Option<TOption>
    : Argument<TOption>
    , IOption
    where TOption : Option<TOption> {
    protected Option(IHasChildren parent, string name, params string[] aliases)
        : base(parent, name, aliases) {
    }

    public sealed override Task<Result> ClearData(CancellationToken ct) {
        Application.Data[Name] = null;
        return OnDataRead(ct);
    }

    public sealed override Task<Result> ReadData(string? value, CancellationToken ct) {
        Application.Data[Name] = value is "null" or "default"
            ? null
            : value;
        return OnDataRead(ct);
    }
}
