namespace DotNetToolbox.CommandLineBuilder;

public abstract class Options : Option {
    protected Options(string name, char alias, string? description = null, Action<Token>? onRead = null)
        : base(name, alias, description, onRead) {
    }
}

public class Options<TValue> : Options, IHasValues<TValue> {
    private readonly ICollection<TValue> _values = new List<TValue>();

    public Options(string name, char alias, string? description = null, Action<Token>? onRead = null)
        : base(name, alias, description, onRead) {
        ValueType = typeof(TValue);
    }

    public Options(string name, string? description = null, Action<Token>? onRead = null)
        : this(name, '\0', description, onRead) {
    }

    public sealed override Type ValueType { get; }
    public IReadOnlyList<TValue> Values => _values.ToArray();

    protected sealed override string[] Read(string[] arguments) {
        var item = (TValue)Convert.ChangeType(arguments[0], typeof(TValue));
        _values.Add(item);
        return arguments[1..];
    }
}
