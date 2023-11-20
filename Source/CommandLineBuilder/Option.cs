namespace DotNetToolbox.CommandLineBuilder;

public abstract class Option : Argument {
    protected Option(string name, char alias, string? description = null, Action<Token>? onRead = null)
        : base(TokenType.Option, name, alias, description, onRead) {
    }
}

public class Option<TValue> : Option, IHasValue<TValue> {
    public Option(string name, char alias, string? description = null, Action<Token>? onRead = null)
        : base(name, alias, description, onRead) {
        ValueType = typeof(TValue);
    }

    public Option(string name, string? description = null, Action<Token>? onRead = null)
        : this(name, '\0', description, onRead) {
    }

    public sealed override Type ValueType { get; }
    public TValue Value { get; private set; } = default!;

    protected sealed override string[] Read(string[] arguments) {
        Value = (TValue)Convert.ChangeType(arguments[0], typeof(TValue));
        return arguments[1..];
    }
}
