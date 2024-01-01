namespace DotNetToolbox.CommandLineBuilder;

public static class CommandBuilder {
    public static CommandBuilder<TRootCommand> From<TRootCommand>()
        where TRootCommand : CommandBase<TRootCommand>
        => new();

    public static CommandBuilder<RootCommand> FromDefaultRoot()
        => From<RootCommand>();
}

public sealed class CommandBuilder<TCommand>
    where TCommand : CommandBase<TCommand> {
    private readonly bool _isRoot;
    private readonly string? _name;
    private readonly string? _description;
    private readonly List<Func<CommandBase, CommandBase>> _steps = [];

    private object? _action;
    private OutputWriter? _writer;

    internal CommandBuilder() {
        _isRoot = true;
    }

    private CommandBuilder(string name, string? description = null) {
        _isRoot = false;
        _name = name;
        _description = description;
    }

    public CommandBuilder<TCommand> WithWriter(OutputWriter writer) {
        _writer = writer;
        return this;
    }

    public CommandBuilder<TCommand> SetAction(Action action) {
        _action = action;
        return this;
    }

    public CommandBuilder<TCommand> SetAction(Action<string[]> action) {
        _action = action;
        return this;
    }

    public CommandBuilder<TCommand> SetAction(Func<Task> action) {
        _action = action;
        return this;
    }

    public CommandBuilder<TCommand> SetAction(Func<string[], Task> action) {
        _action = action;
        return this;
    }

    public CommandBuilder<TCommand> SetAction(Func<CancellationToken, Task> action) {
        _action = action;
        return this;
    }

    public CommandBuilder<TCommand> SetAction(Func<string[], CancellationToken, Task> action) {
        _action = action;
        return this;
    }

    public CommandBuilder<TCommand> SetAction(Action<TCommand> action) {
        _action = action;
        return this;
    }

    public CommandBuilder<TCommand> SetAction(Action<TCommand, string[]> action) {
        _action = action;
        return this;
    }

    public CommandBuilder<TCommand> SetAction(Func<TCommand, Task> action) {
        _action = action;
        return this;
    }

    public CommandBuilder<TCommand> SetAction(Func<TCommand, string[], Task> action) {
        _action = action;
        return this;
    }

    public CommandBuilder<TCommand> SetAction(Func<TCommand, CancellationToken, Task> action) {
        _action = action;
        return this;
    }

    public CommandBuilder<TCommand> SetAction(Func<TCommand, string[], CancellationToken, Task> action) {
        _action = action;
        return this;
    }

    public CommandBuilder<TCommand> AddFlag(string name, string? description = null, bool existsIfSet = false, Action<Token>? onRead = null)
        => AddFlag(name, '\0', description, existsIfSet, onRead);

    public CommandBuilder<TCommand> AddFlag(string name, char alias, string? description = null, bool existsIfSet = false, Action<Token>? onRead = null)
        => AddChild(new Flag(name, alias, description, existsIfSet, onRead));

    public CommandBuilder<TCommand> AddOptions<T>(string name, string? description = null, Action<Token>? onRead = null)
        => AddOptions<T>(name, '\0', description, onRead);

    public CommandBuilder<TCommand> AddOptions<T>(string name, char alias, string? description = null, Action<Token>? onRead = null)
        => AddChild(new Options<T>(name, alias, description, onRead));

    public CommandBuilder<TCommand> AddOption<T>(string name, string? description = null, Action<Token>? onRead = null)
        => AddOption<T>(name, '\0', description, onRead);

    public CommandBuilder<TCommand> AddOption<T>(string name, char alias, string? description = null, Action<Token>? onRead = null)
        => AddChild(new Option<T>(name, alias, description, onRead));

    public CommandBuilder<TCommand> AddParameter<T>(string name, string? description = null, Action<Token>? onRead = null)
        => AddChild(new Parameter<T>(name, description, onRead));

    public CommandBuilder<TCommand> AddChild(string name, string? description = null)
        => AddChild(name, description, null);

    public CommandBuilder<TCommand> AddChild(string name, Action<CommandBuilder<Command>> build)
        => AddChild(name, null, build);

    public CommandBuilder<TCommand> AddChild(string name, string? description, Action<CommandBuilder<Command>>? build)
        => AddChild<Command>(name, description, build);

    public CommandBuilder<TCommand> AddChild<TChildCommand>(string name, string? description = null)
        where TChildCommand : CommandBase<TChildCommand>
        => AddChild<TChildCommand>(name, description, null);

    public CommandBuilder<TCommand> AddChild<TChildCommand>(string name, Action<CommandBuilder<TChildCommand>> build)
        where TChildCommand : CommandBase<TChildCommand>
        => AddChild(name, null, build);

    public CommandBuilder<TCommand> AddChild<TChildCommand>(string name, string? description, Action<CommandBuilder<TChildCommand>>? build)
        where TChildCommand : CommandBase<TChildCommand> {
        CommandBuilder<TChildCommand> builder = new(name, description);
        build?.Invoke(builder);
        AddChild(builder.Build());
        return this;
    }

    public CommandBuilder<TCommand> AddChild(Token token) {
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
        switch (_action) {
            case Action action: command.SetAction(action); break;
            case Action<string[]> action: command.SetAction(action); break;
            case Func<Task> action: command.SetAction(action); break;
            case Func<string[], Task> action: command.SetAction(action); break;
            case Func<CancellationToken, Task> action: command.SetAction(action); break;
            case Func<string[], CancellationToken, Task> action: command.SetAction(action); break;
            case Action<TCommand> action: command.SetAction(action); break;
            case Action<TCommand, string[]> action: command.SetAction(action); break;
            case Func<TCommand, Task> action: command.SetAction(action); break;
            case Func<TCommand, string[], Task> action: command.SetAction(action); break;
            case Func<TCommand, CancellationToken, Task> action: command.SetAction(action); break;
            case Func<TCommand, string[], CancellationToken, Task> action: command.SetAction(action); break;
        }
        return (TCommand)_steps.Aggregate((CommandBase)command, (current, step) => step(current));
    }
}
