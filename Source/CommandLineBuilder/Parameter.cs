namespace DotNetToolbox.CommandLineBuilder;

public abstract class Parameter : Argument, IHasValue {
    protected Parameter(string name, string? description = null, Action<Token>? onRead = null)
        : base(TokenType.Parameter, name, '\0', description, onRead) {
    }
}

public class Parameter<TValue> : Parameter, IHasValue<TValue> {
    public Parameter(string name, string? description = null, Action<Token>? onRead = null)
    : base(name, description, onRead) {
        ValueType = typeof(TValue);
    }

    public sealed override Type ValueType { get; }
    public TValue Value { get; private set; } = default!;

    protected sealed override string[] Read(string[] arguments) {
        Value = (TValue)Convert.ChangeType(arguments[0], typeof(TValue));
        return arguments;
    }
}
