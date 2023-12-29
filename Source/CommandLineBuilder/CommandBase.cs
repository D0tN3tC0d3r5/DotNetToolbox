namespace DotNetToolbox.CommandLineBuilder;

public abstract class CommandBase(TokenType type, string name, string? description = null)
    : Token(type, name, description), IDisposable {

    private bool _isDisposed;
    protected virtual void ExecuteDispose() {
        var disposables = Tokens.OfType<IDisposable>().ToArray();
        foreach (var disposable in disposables) {
            disposable.Dispose();
        }
    }

    public void Dispose() {
        if (_isDisposed) return;
        ExecuteDispose();
        GC.SuppressFinalize(this);
        _isDisposed = true;
    }

    internal abstract Task ExecuteCommand(string[] arguments, CancellationToken ct);
    
    internal IList<Token> Tokens { get; } = new List<Token>();

    public void Add<T>(T token) where T : Token {
        EnsureUniqueness(token);
        token.Parent = this;
        token.Writer = Writer;
        Tokens.Add(token);
    }

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

    private static TValue? GetArgumentValue<TValue>(Argument? argument, string source, string parameterName, bool isRequired, TValue? defaultValue = default)
        => argument switch {
            null => throw new ArgumentException($"Argument '{source}' not found.", parameterName),
            { IsSet: false } when isRequired => throw new ArgumentException($"Required {argument.TokenType} '{source}' not set.", parameterName),
            { IsSet: false } => defaultValue,
            IHasValue<TValue> typedArgument => typedArgument.Value,
            _ => throw ExceptionHelper.CreateGetCastException<TValue>(argument),
    };

    public bool IsFlagSet(string nameOrAlias) {
        ValidationHelper.ValidateName(nameOrAlias);
        var argument = Tokens.OfType<Argument>().FirstOrDefaultByNameOrAlias(nameOrAlias);
        return argument switch {
            null => throw new ArgumentException($"Argument '{nameOrAlias}' not found.", nameof(nameOrAlias)),
            Flag flag => flag.Value,
            _ => throw new ArgumentException($"Argument '{nameOrAlias}' is not a flag.", nameof(nameOrAlias)),
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
        var subCommand = Tokens.OfType<CommandBase>().FirstOrDefaultByName(name);
        if (subCommand is null) return false;

        await subCommand.ExecuteCommand(arguments[1..], ct);
        return true;
    }
}

public abstract class CommandBase<TCommand>
    : CommandBase
    where TCommand : CommandBase<TCommand> {
    private Func<string[], CancellationToken, Task> _command;

    protected CommandBase(TokenType type, string name, string? description = null)
        : base(type, name, description) {
        _command = (_, ct) => Task.Run(() => Writer.WriteHelp(this), ct);
    }

    public void SetAction(Action action)
        => _command = (_, ct) => Task.Run(action, ct);

    public void SetAction(Action<string[]> action)
        => _command = (args, ct) => Task.Run(() => action(args), ct);

    public void SetAction(Action<TCommand> action)
        => _command = (_, ct) => Task.Run(() => action((TCommand)this), ct);

    public void SetAction(Action<TCommand, string[]> action)
        => _command = (args, ct) => Task.Run(() => action((TCommand)this, args), ct);

    public void SetAction(Func<Task> action)
        => _command = (_, _) => action();

    public void SetAction(Func<string[], Task> action)
        => _command = (args, _) => action(args);

    public void SetAction(Func<CancellationToken, Task> action)
        => _command = (_, ct) => action(ct);

    public void SetAction(Func<string[], CancellationToken, Task> action)
        => _command = action;

    public void SetAction(Func<TCommand, Task> action)
        => _command = (_, _) => action((TCommand)this);

    public void SetAction(Func<TCommand, string[], Task> action)
        => _command = (args, _) => action((TCommand)this, args);

    public void SetAction(Func<TCommand, CancellationToken, Task> action)
        => _command = (_, ct) => action((TCommand)this, ct);

    public void SetAction(Func<TCommand, string[], CancellationToken, Task> action)
        => _command = (args, ct) => action((TCommand)this, args, ct);

    public void Execute(params string[] arguments)
        => ExecuteAsync(arguments).GetAwaiter().GetResult();

    public Task ExecuteAsync(params string[] arguments)
        => ExecuteAsync(arguments, default);

    public Task ExecuteAsync(string[] arguments, CancellationToken ct)
        => ExecuteCommand(arguments, ct);

    internal override async Task ExecuteCommand(string[] arguments, CancellationToken ct) {
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
            await _command(arguments, ct);
        }
        catch (Exception ex) {
            Writer.WriteError($"An error occurred while executing command '{Name}'.", ex);
        }
    }
}
