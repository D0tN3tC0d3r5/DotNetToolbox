namespace DotNetToolbox.CommandLineBuilder;

public abstract class Options(string name, char alias, string? description = null, Action<Token>? onRead = null)
    : Option(name,
             alias,
             description,
             onRead);

public class Options<TValue>(string name, char alias, string? description = null, Action<Token>? onRead = null)
    : Options(name,
              alias,
              description,
              onRead),
      IHasValues<TValue> {
    private readonly List<TValue> _values = [];

    public Options(string name, string? description = null, Action<Token>? onRead = null)
        : this(name, '\0', description, onRead) {
    }

    public sealed override Type ValueType { get; } = typeof(TValue);
    public IReadOnlyList<TValue> Values => _values.ToArray();

    protected sealed override string[] Read(string[] arguments) {
        var item = (TValue)Convert.ChangeType(arguments[0], typeof(TValue));
        _values.Add(item);
        return arguments[1..];
    }
}
