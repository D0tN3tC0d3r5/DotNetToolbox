namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public sealed class Option
    : Option<Option> {
    public Option(IHasChildren parent, string name, params string[] aliases)
        : base(parent, name, aliases) {
    }

    protected override Task<Result> OnRead(CancellationToken ct)
        => SuccessTask();
}

public abstract class Option<TOption>
    : Argument<TOption>
    , IOption, IHasValue<string>
    where TOption : Option<TOption> {
    protected Option(IHasChildren parent, string name, params string[] aliases)
        : base(parent, "Option", name, aliases) {
    }

    public string? Value { get; private set; }
    public Task<Result> SetValue(string input, CancellationToken ct) {
        Value = input == "null" ? null : input;
        return OnRead(ct);
    }
}
