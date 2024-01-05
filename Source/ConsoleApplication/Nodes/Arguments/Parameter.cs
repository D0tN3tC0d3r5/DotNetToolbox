namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public class Parameter
    : Parameter<object> {
    protected Parameter(IHasChildren parent, string name, object? defaultValue = null)
        : base(parent, name, defaultValue) {
    }

    protected override Task<Result> OnRead(CancellationToken ct)
        => SuccessTask();
}

public class Parameter<TValue>
    : Parameter<Parameter<TValue>, TValue> {
    protected Parameter(IHasChildren parent, string name, TValue? defaultValue = default)
        : base(parent, name, defaultValue) {
    }

    protected override Task<Result> OnRead(CancellationToken ct)
        => SuccessTask();
}

public abstract class Parameter<TParameter, TValue>
    : Argument<TParameter>
    , IParameter, IHasValue<TValue>
    where TParameter : Parameter<TParameter, TValue> {
    protected Parameter(IHasChildren parent, string name, TValue? defaultValue = default)
        : base(parent, "Parameter", name) {
        Order = parent.Children.OfType<IParameter>().Count();
        DefaultValue = defaultValue;
    }

    public int Order { get; }
    public object? DefaultValue { get; }
    public TValue? Value { get; set; }

    public async Task<Result> SetValue(string input, CancellationToken ct) {
        try {
            Value = (TValue)Convert.ChangeType(input, typeof(TValue));
        }
        catch (Exception ex) {
            Logger.LogError(ex, "Failed to convert {input} to {type}", input, typeof(TValue));
            return Error($"Failed to convert {input} to {typeof(TValue).Name}");
        }

        return await OnRead(ct);
    }
}
