namespace DotNetToolbox.CommandLineBuilder;

public abstract partial class Argument : Token {
    private readonly Action<Token>? _onRead;
    private static readonly Regex _validAlias = MyRegex();

    protected Argument(TokenType type, string name, char alias = '\0', string? description = null, Action<Token>? onRead = null)
        : base(type, name, description) {
        Alias = alias;
        if (Alias != '\0' && !_validAlias.IsMatch(Alias.ToString()))
            throw new ArgumentException($"'{alias}' is not a valid alias. An alias must be a letter or number.", nameof(alias));
        _onRead = onRead;
    }

    public abstract Type ValueType { get; }
    internal bool IsSet { get; private set; }
    internal char Alias { get; }

    internal bool Is(char candidate) => Alias != '\0' && Alias == candidate;

    internal string[] Read(CommandBase caller, string[] arguments) {
        try {
            arguments = Read(arguments);
            OnRead(caller, arguments.ToArray());
            IsSet = true;
            return arguments;
        }
        catch (Exception ex) {
            Writer.WriteError($"An error occurred while reading {TokenType.ToString().ToLower()} '{Name}'.", ex);
            throw;
        }
    }

    protected abstract string[] Read(string[] arguments);

    protected virtual void OnRead(CommandBase caller, IEnumerable<string> arguments) => _onRead?.Invoke(this);

    [GeneratedRegex("^[a-zA-Z0-9]$", RegexOptions.Compiled)]
    private static partial Regex MyRegex();
}
