namespace DotNetToolbox.ConsoleApplication.Nodes;

public sealed class Parameter
    : Parameter<Parameter> {
    public Parameter(IHasChildren parent, string name)
        : this(parent, name, null!) { }

    public Parameter(IHasChildren parent, string name, Action<Parameter> configure)
        : base(parent, name, configure) {
    }
}

public abstract class Parameter<TParameter>
    : Node<TParameter>
    , IParameter
    where TParameter : Parameter<TParameter> {
    protected Parameter(IHasChildren parent, string name)
        : this(parent, name, null!) {
        Order = parent.Children.OfType<IParameter>()
                      .Count();
    }

    protected Parameter(IHasChildren parent, string name, Action<TParameter> configure)
        : base(parent, name, configure) {
        Order = parent.Children.OfType<IParameter>().Count();
    }

    public string? DefaultValue {
        get;
        set {
            field = value;
            Parent.Context[Name] = field!;
        }
    }

    public int Order { get; }
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

    protected virtual Task<Result> Execute(CancellationToken ct = default) => Task.FromResult(Success());
}
