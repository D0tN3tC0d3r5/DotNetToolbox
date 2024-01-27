namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

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
    public bool IsRequired => _defaultValue is not null;
    public int Order { get; }

    public sealed override Task<Result> ExecuteAsync(IReadOnlyList<string> args, CancellationToken ct = default) {
        Application.Data[Name] = args[0] switch {
            null or "default" => _defaultValue,
            "null" => null,
            _ => args[0],
        };
        IsSet = true;
        return SuccessTask();
    }
}
