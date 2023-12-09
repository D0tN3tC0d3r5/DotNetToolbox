namespace DotNetToolbox.CommandLineBuilder;

public abstract class Parameter(string name, string? description = null, Action<Token>? onRead = null) : Argument(TokenType.Parameter,
                                                                                                                  name,
                                                                                                                  '\0',
                                                                                                                  description,
                                                                                                                  onRead),
                                                                                                         IHasValue;

public class Parameter<TValue>(string name, string? description = null, Action<Token>? onRead = null) : Parameter(name, description, onRead), IHasValue<TValue> {
    public sealed override Type ValueType { get; } = typeof(TValue);
    public TValue Value { get; private set; } = default!;

    protected sealed override string[] Read(string[] arguments) {
        Value = (TValue)Convert.ChangeType(arguments[0], typeof(TValue));
        return arguments;
    }
}
