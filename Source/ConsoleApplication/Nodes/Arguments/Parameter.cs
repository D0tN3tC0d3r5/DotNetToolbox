namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public sealed class Parameter
    : Parameter<Parameter> {
    public Parameter(IHasChildren parent, string name, string? defaultValue = null)
        : base(parent, name, defaultValue) {
    }

    protected override Task<Result> OnRead(CancellationToken ct)
        => SuccessTask();
}

//public class Parameter<TValue>
//    : Parameter<Parameter<TValue>, TValue> {
//    protected Parameter(IHasChildren parent, string name, TValue? defaultValue = default)
//        : base(parent, name, defaultValue) {
//    }

//    protected override Task<Result> OnRead(CancellationToken ct)
//        => SuccessTask();
//}

public abstract class Parameter<TParameter>
    : Argument<TParameter>
    , IParameter, IHasValue<string>
    where TParameter : Parameter<TParameter> {
    protected Parameter(IHasChildren parent, string name, string? defaultValue = default)
        : base(parent, "Parameter", name) {
        Order = parent.Children.OfType<IParameter>().Count();
        DefaultValue = defaultValue;
    }

    public int Order { get; }
    public string? DefaultValue { get; }
    public string? Value { get; set; }

    public Task<Result> SetValue(string input, CancellationToken ct) {
        Value = input == "null" ? null : input;
        return OnRead(ct);
    }
}
