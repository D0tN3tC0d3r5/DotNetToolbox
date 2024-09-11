namespace DotNetToolbox.ConsoleApplication.Nodes;

public sealed class Parameter(IHasChildren parent, string name, string? defaultValue = null)
        : Parameter<Parameter>(parent, name, defaultValue);

public abstract class Parameter<TParameter>
    : Node<TParameter>, IParameter
    where TParameter : Parameter<TParameter> {
    protected Parameter(IHasChildren parent, string name, string? defaultValue = default)
        : base(parent, name) {
        DefaultValue = defaultValue;
        parent.Map[Name] = DefaultValue!;
        Order = parent.Children.OfType<IParameter>().Count();
    }

    public int Order { get; }
    public string? DefaultValue { get; }
    public bool IsRequired => DefaultValue is null;
    public bool IsSet { get; private set; }

    Task<Result> IParameter.Read(string? value, IMap context, CancellationToken ct) {
        context[Name] = value switch {
            null or "default" => DefaultValue!,
            "null" => null!,
            ['"', .. var text, '"'] => text,
            _ => value,
        };
        IsSet = true;
        return Execute(ct);
    }

    protected virtual Task<Result> Execute(CancellationToken ct = default) => SuccessTask();
}
