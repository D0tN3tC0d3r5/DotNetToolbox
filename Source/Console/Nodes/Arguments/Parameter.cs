namespace ConsoleApplication.Nodes.Arguments;

public sealed class Parameter
    : Parameter<Parameter> {
    public Parameter(IHasChildren parent, string name, string? defaultValue = null)
        : base(parent, name, defaultValue) {
    }
}

public abstract class Parameter<TParameter>
    : Argument<TParameter>
    , IParameter
    where TParameter : Parameter<TParameter> {
    private readonly string? _defaultValue;

    protected Parameter(IHasChildren parent, string name, string? defaultValue = default)
        : base(parent, name) {
        _defaultValue = defaultValue;
        Application.Data[Name] = _defaultValue;
        Order = parent.Children.OfType<IParameter>().Count();
    }

    public bool IsSet { get; private set; }
    public bool IsRequired => _defaultValue is not null;
    public int Order { get; }

    public sealed override Task<Result> ClearData(CancellationToken ct) {
        Application.Data[Name] = _defaultValue;
        IsSet = _defaultValue is not null;
        return OnDataCleared(ct);
    }

    public sealed override Task<Result> ReadData(string? value, CancellationToken ct) {
        Application.Data[Name] = value switch {
            null or "default" => _defaultValue,
            "null" => null,
            _ => value,
        };
        IsSet = true;
        return OnDataRead(ct);
    }
}
