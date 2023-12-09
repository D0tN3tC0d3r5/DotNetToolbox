namespace DotNetToolbox.CommandLineBuilder;

public class Flag(string name,
                  char alias,
                  string? description = null,
                  bool existsIfTrue = false,
                  Action<Token>? onRead = null)
    : Argument(TokenType.Flag,
               name,
               alias,
               description,
               onRead),
      IHasValue<bool> {
    public Flag(string name, string? description = null, bool existsIfSet = false, Action<Token>? onRead = null)
        : this(name, '\0', description, existsIfSet, onRead) {
    }

    public sealed override Type ValueType { get; } = typeof(bool);
    public bool Value => IsSet;
    public bool ExitsIfTrue { get; } = existsIfTrue;
    protected sealed override string[] Read(string[] arguments) => arguments;
}
