namespace DotNetToolbox.ConsoleApplication.Nodes;

public sealed class Parameter(IHasChildren parent, string name, string? defaultValue = null)
        : Parameter<Parameter>(parent, name, defaultValue);

public abstract class Parameter<TParameter>
    : Node<TParameter>, IParameter
    where TParameter : Parameter<TParameter> {
    private readonly string? _defaultValue;

    protected Parameter(IHasChildren parent, string name, string? defaultValue = default)
        : base(parent, name) {
        _defaultValue = defaultValue;
        Application.Data[Name] = _defaultValue;
        Order = parent.Children.OfType<IParameter>().Count();
    }

    public bool IsSet { get; private set; }
    public bool IsRequired => _defaultValue is null;
    public int Order { get; }

    Task<Result> IParameter.Read(string? value, CancellationToken ct) {
        Application.Data[Name] = value switch {
            null or "default" => _defaultValue,
            "null" => null,
            ['"', .. var text, '"'] => text,
            _ => value,
        };
        IsSet = true;
        return Execute(ct);
    }

    protected virtual Task<Result> Execute(CancellationToken ct = default) => SuccessTask();
}
