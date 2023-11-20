namespace DotNetToolbox.CommandLineBuilder;

public class Flag : Argument, IHasValue<bool> {
    public Flag(string name, char alias, string? description = null, bool existsIfTrue = false, Action<Token>? onRead = null)
        : base(TokenType.Flag, name, alias, description, onRead) {
        ValueType = typeof(bool);
        ExitsIfTrue = existsIfTrue;
    }

    public Flag(string name, string? description = null, bool existsIfSet = false, Action<Token>? onRead = null)
        : this(name, '\0', description, existsIfSet, onRead) {
    }

    public sealed override Type ValueType { get; }
    public bool Value => IsSet;
    public bool ExitsIfTrue { get; }
    protected sealed override string[] Read(string[] arguments) => arguments;
}
