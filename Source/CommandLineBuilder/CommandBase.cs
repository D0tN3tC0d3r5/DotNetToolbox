namespace DotNetToolbox.CommandLineBuilder;

public abstract class CommandBase : Token, IAsyncDisposable {
    protected CommandBase(string name, string? description = null)
        : base(TokenType.Command, name, description) {
    }

    protected virtual ValueTask DisposeAsyncCore()
        => ValueTask.CompletedTask;

    public async ValueTask DisposeAsync() {
        await DisposeAsyncCore();
        GC.SuppressFinalize(this);
    }

    internal IList<Token> Tokens { get; } = new List<Token>();

    public void Add<T>(T token) where T : Token {
        EnsureUniqueness(token);
        token.Parent = this;
        token.Writer = Writer;
        Tokens.Add(token);
    }

    public abstract Task Execute(string[] arguments, CancellationToken ct);

    public IReadOnlyList<T> GetValuesOrDefault<T>(string nameOrAlias, IReadOnlyList<T>? defaultValue = null) {
        ValidationHelper.ValidateName(nameOrAlias);
        var argument = Tokens.OfType<Argument>().FirstOrDefaultByNameOrAlias(nameOrAlias);
        return GetArgumentValues(argument, nameOrAlias, nameof(nameOrAlias), false, defaultValue);
    }

    public IReadOnlyList<T> GetValues<T>(string nameOrAlias) {
        ValidationHelper.ValidateName(nameOrAlias);
        var argument = Tokens.OfType<Argument>().FirstOrDefaultByNameOrAlias(nameOrAlias);
        return GetArgumentValues<T>(argument, nameOrAlias, nameof(nameOrAlias), true);
    }

    private static IReadOnlyList<TValue> GetArgumentValues<TValue>(Argument? argument, string source, string parameterName, bool isRequired, IReadOnlyList<TValue>? defaultValue = null) => argument switch {
        null => throw new ArgumentException($"Argument '{source}' not found.", parameterName), { IsSet: false } when isRequired => throw new ArgumentException($"{argument.TokenType} '{source}' not set.", parameterName), { IsSet: false } => defaultValue ?? Array.Empty<TValue>(),
        IHasValues<TValue> typedArgument => typedArgument.Values,
        _ => throw ExceptionHelper.CreateGetCastException<TValue>(argument),
    };

    public T? GetValueOrDefault<T>(string nameOrAlias, T? defaultValue = default) {
        ValidationHelper.ValidateName(nameOrAlias);
        var argument = Tokens.OfType<Argument>().Except(Tokens.OfType<Options>()).FirstOrDefaultByNameOrAlias(nameOrAlias);
        return GetArgumentValue(argument, nameOrAlias, nameof(nameOrAlias), false, defaultValue);
    }

    public T? GetValueOrDefault<T>(uint index, T? defaultValue = default) {
        var parameters = Tokens.OfType<Parameter>().ToArray();
        ValidationHelper.ValidateParameterIndex(parameters, index);
        var parameter = parameters[(int)index];
        return GetArgumentValue(parameter, index.ToString(), nameof(index), false, defaultValue);
    }

    public T GetValue<T>(string nameOrAlias) {
        ValidationHelper.ValidateName(nameOrAlias);
        var argument = Tokens.OfType<Argument>().Except(Tokens.OfType<Options>()).FirstOrDefaultByNameOrAlias(nameOrAlias);
        return GetArgumentValue<T>(argument, nameOrAlias, nameof(nameOrAlias), true)!;
    }

    public T GetValue<T>(uint index) {
        var parameters = Tokens.OfType<Parameter>().ToArray();
        ValidationHelper.ValidateParameterIndex(parameters, index);
        var parameter = parameters[(int)index];
        return GetArgumentValue<T>(parameter, index.ToString(), nameof(index), true)!;
    }

    private static TValue? GetArgumentValue<TValue>(Argument? argument, string source, string parameterName, bool isRequired, TValue? defaultValue = default) => argument switch {
        null => throw new ArgumentException($"Argument '{source}' not found.", parameterName), { IsSet: false } when isRequired => throw new ArgumentException($"{argument.TokenType} '{source}' not set.", parameterName), { IsSet: false } => defaultValue,
        IHasValue<TValue> typedArgument => typedArgument.Value,
        _ => throw ExceptionHelper.CreateGetCastException<TValue>(argument)
    };

    public bool IsFlagSet(string nameOrAlias) {
        ValidationHelper.ValidateName(nameOrAlias);
        var argument = Tokens.OfType<Argument>().FirstOrDefaultByNameOrAlias(nameOrAlias);
        return argument switch {
            null => throw new ArgumentException($"Argument '{nameOrAlias}' not found.", nameof(nameOrAlias)),
            Flag flag => flag.Value,
            _ => throw new ArgumentException($"Argument '{nameOrAlias}' is not a flag.", nameof(nameOrAlias))
        };
    }

    internal string Path => (Parent is null ? "" : Parent.Path + " ") + Name;

    private void EnsureUniqueness(Token token) {
        if (Tokens.Any(i => i.Is(token.Name)))
            throw new InvalidOperationException($"An argument with name '{token.Name}' already exists.");
        if (token is not Argument at) return;
        if (Tokens.OfType<Argument>().Any(i => i.Is(at.Alias)))
            throw new InvalidOperationException($"An argument with alias '{at.Alias}' already exists.");
    }

    protected bool TryReadOption(string[] arguments, out string[] output) {
        output = arguments;

        var option = Tokens.OfType<Option>().FirstOrDefaultByNameOrAlias(output[0]);
        if (option is null) return false;

        output = option.Read(this, output[1..]);
        return true;
    }

    protected bool TryReadFlag(string[] arguments, out string[] output, out bool exit) {
        output = arguments;
        exit = false;

        var flag = Tokens.OfType<Flag>().FirstOrDefaultByNameOrAlias(output[0]);
        if (flag is null) return false;

        output = flag.Read(this, output[1..]);
        exit = flag.ExitsIfTrue;
        return true;
    }

    protected void ReadParameters(string[] arguments, out string[] output) {
        output = arguments;
        foreach (var parameter in Tokens.OfType<Parameter>()) {
            parameter.Read(this, output);
            output = output[1..];
        }
    }

    protected async Task<bool> TryExecuteChildCommand(string[] arguments, CancellationToken ct) {
        var name = arguments[0].Trim();
        var subCommand = Tokens.OfType<Command>().FirstOrDefaultByName(name);
        if (subCommand is null) return false;

        await subCommand.Execute(arguments[1..], ct);
        return true;
    }
}

public abstract class CommandBase<TCommand> : CommandBase
    where TCommand : CommandBase<TCommand> {
    private static Task DefaultAsyncAction(TCommand c) => Task.Run(() => c.Writer.WriteHelp(c));

    protected CommandBase(string name, string? description = null)
        : base(name, description) {
    }

    protected override ValueTask DisposeAsyncCore() {
        OnExecute = null;
        return ValueTask.CompletedTask;
    }

    public event Func<TCommand, CancellationToken, Task>? OnExecute;

    public sealed override async Task Execute(string[] arguments, CancellationToken ct) {
        try {
            while (arguments.Length > 0) {
                if (TryReadFlag(arguments, out arguments, out var exit)) {
                    if (exit) return;
                    continue;
                }
                if (TryReadOption(arguments, out arguments)) continue;
                if (await TryExecuteChildCommand(arguments, ct)) return;
                break;
            }

            ReadParameters(arguments, out arguments);
            await (OnExecute?.Invoke((TCommand)this, ct) ?? DefaultAsyncAction((TCommand)this));
        }
        catch (Exception ex) {
            Writer.WriteError($"An error occurred while executing command '{Name}'.", ex);
        }
    }

    public Task Execute(params string[] arguments)
        => Execute(arguments, default);
}
