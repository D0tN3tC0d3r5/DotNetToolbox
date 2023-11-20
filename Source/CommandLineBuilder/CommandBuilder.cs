namespace DotNetToolbox.CommandLineBuilder;

public static class CommandBuilder {
    public static CommandBuilder<TRootCommand> From<TRootCommand>(OutputWriter? writer = null)
        where TRootCommand : CommandBase<TRootCommand>
        => new(writer);

    public static CommandBuilder<RootCommand> FromDefaultRoot(OutputWriter? writer = null) => From<RootCommand>(writer);
}

public sealed class CommandBuilder<TCommand>
    where TCommand : CommandBase<TCommand> {
    private readonly bool _isRoot;
    private readonly string? _name;
    private readonly string? _description;
    private readonly OutputWriter? _writer;

    private Func<TCommand, CancellationToken, Task>? _onExecute;

    private readonly ICollection<Func<CommandBase, CommandBase>> _steps = new List<Func<CommandBase, CommandBase>>();

    internal CommandBuilder(OutputWriter? writer = null) {
        _isRoot = true;
        _writer = writer ?? new();
    }

    private CommandBuilder(string name, string? description = null) {
        _isRoot = false;
        _name = name;
        _description = description;
    }

    public CommandBuilder<TCommand> OnExecute(Action onExecute) {
        _onExecute = (_, ct) => Task.Run(onExecute, ct);
        return this;
    }

    public CommandBuilder<TCommand> OnExecute(Action<CommandBase> onExecute) {
        _onExecute = (cmd, ct) => Task.Run(() => onExecute(cmd), ct);
        return this;
    }

    public CommandBuilder<TCommand> OnExecute(Func<Task> onExecute) {
        _onExecute = (_, _) => onExecute();
        return this;
    }

    public CommandBuilder<TCommand> OnExecute(Func<CommandBase, Task> onExecute) {
        _onExecute = (cmd, _) => onExecute(cmd);
        return this;
    }

    public CommandBuilder<TCommand> OnExecute(Func<CancellationToken, Task> onExecute) {
        _onExecute = (_, ct) => onExecute(ct);
        return this;
    }

    public CommandBuilder<TCommand> OnExecute(Func<CommandBase, CancellationToken, Task> onExecute) {
        _onExecute = onExecute;
        return this;
    }

    public CommandBuilder<TCommand> AddFlag(string name, string? description = null, bool existsIfSet = false, Action<Token>? onRead = null)
        => AddFlag(name, '\0', description, existsIfSet, onRead);

    public CommandBuilder<TCommand> AddFlag(string name, char alias, string? description = null, bool existsIfSet = false, Action<Token>? onRead = null)
        => Add(new Flag(name, alias, description, existsIfSet, onRead));

    public CommandBuilder<TCommand> AddOptions<T>(string name, string? description = null, Action<Token>? onRead = null)
        => AddOptions<T>(name, '\0', description, onRead);

    public CommandBuilder<TCommand> AddOptions<T>(string name, char alias, string? description = null, Action<Token>? onRead = null)
        => Add(new Options<T>(name, alias, description, onRead));

    public CommandBuilder<TCommand> AddOption<T>(string name, string? description = null, Action<Token>? onRead = null)
        => AddOption<T>(name, '\0', description, onRead);

    public CommandBuilder<TCommand> AddOption<T>(string name, char alias, string? description = null, Action<Token>? onRead = null)
        => Add(new Option<T>(name, alias, description, onRead));

    public CommandBuilder<TCommand> AddParameter<T>(string name, string? description = null, Action<Token>? onRead = null)
        => Add(new Parameter<T>(name, description, onRead));

    public CommandBuilder<TCommand> AddChildCommand(string name, string? description = null)
        => AddChildCommand(name, description, null);

    public CommandBuilder<TCommand> AddChildCommand(string name, Action<CommandBuilder<Command>> build)
        => AddChildCommand(name, null, build);

    public CommandBuilder<TCommand> AddChildCommand(string name, string? description, Action<CommandBuilder<Command>>? build)
        => AddChildCommand<Command>(name, description, build);

    public CommandBuilder<TCommand> AddChildCommand<TChildCommand>(string name, string? description = null)
        where TChildCommand : CommandBase<TChildCommand>
        => AddChildCommand<TChildCommand>(name, description, null);

    public CommandBuilder<TCommand> AddChildCommand<TChildCommand>(string name, Action<CommandBuilder<TChildCommand>> build)
        where TChildCommand : CommandBase<TChildCommand>
        => AddChildCommand(name, null, build);

    public CommandBuilder<TCommand> AddChildCommand<TChildCommand>(string name, string? description, Action<CommandBuilder<TChildCommand>>? build)
        where TChildCommand : CommandBase<TChildCommand> {
        CommandBuilder<TChildCommand> builder = new(name, description);
        build?.Invoke(builder);
        Add(builder.Build());
        return this;
    }

    public CommandBuilder<TCommand> Add(Token token) {
        _steps.Add(parent => {
            parent.Add(token);
            return parent;
        });
        return this;
    }

    public TCommand Build() {
        var command = _isRoot
            ? (TCommand)Activator.CreateInstance(typeof(TCommand), _writer)!
            : (TCommand)Activator.CreateInstance(typeof(TCommand), _name!, _description)!;
        if (_onExecute is not null) command.OnExecute += _onExecute;
        return (TCommand)_steps.Aggregate((CommandBase)command, (current, step) => step(current));
    }
}
