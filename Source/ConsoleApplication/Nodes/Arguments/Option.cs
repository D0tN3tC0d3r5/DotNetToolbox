namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public class Option
    : Option<object> {
    protected Option(IHasChildren parent, string name, params string[] aliases)
        : base(parent, name, aliases) {
    }

    protected override Task<Result> OnRead(CancellationToken ct)
        => SuccessTask();
}

public class Option<TValue>
    : Option<Option<TValue>, TValue> {
    protected Option(IHasChildren parent, string name, params string[] aliases)
        : base(parent, name, aliases) {
    }

    protected override Task<Result> OnRead(CancellationToken ct)
        => SuccessTask();
}

public abstract class Option<TOption, TValue>
    : Argument<TOption>
    , IOption, IHasValue<TValue>
    where TOption : Option<TOption, TValue> {
    protected Option(IHasChildren parent, string name, params string[] aliases)
        : base(parent, "Option", name, aliases) {
    }

    public TValue? Value { get; private set; }
    public async Task<Result> SetValue(string input, CancellationToken ct) {
        try {
            Value = (TValue)Convert.ChangeType(input, typeof(TValue));
        }
        catch (Exception ex) {
            Logger.LogError(ex, "Failed to convert {input} to {type}", input, typeof(TValue).Name);
            return Error($"Failed to convert {input} to {typeof(TValue).Name}");
        }

        return await OnRead(ct);
    }
}
