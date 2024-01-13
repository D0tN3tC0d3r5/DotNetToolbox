namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

public abstract class Command<TCommand>
    : Executable<TCommand>
    , ICommand
    where TCommand : Command<TCommand> {

    protected Command(IHasChildren node, string name, params string[] aliases)
        : base(node, name, aliases) {
        AddAction<HelpAction>();
    }

    public ICollection<INode> Children { get; } = [];

    public override async Task<Result> ExecuteAsync(string[] args, CancellationToken ct) {
        var result = await InputReader.ParseTokens([.. Children], args, ct);
        return result.IsSuccess
                   ? await ExecuteAsync(ct)
                   : result;
    }

    public TCommand AddCommand<TChildCommand>()
        where TChildCommand : Command<TChildCommand> {
        Children.Add(CreateInstance.Of<TChildCommand>(Application.ServiceProvider, this));
        return (TCommand)this;
    }

    public TCommand AddAction<TAction>()
        where TAction : Executable<TAction> {
        Children.Add(CreateInstance.Of<TAction>(Application.ServiceProvider, this));
        return (TCommand)this;
    }

    public TCommand AddOption(string name, params string[] aliases) {
        Children.Add(CreateInstance.Of<Option>(this, name, aliases));
        return (TCommand)this;
    }

    public TCommand AddOption<TOption>()
        where TOption : Option<TOption> {
        Children.Add(CreateInstance.Of<TOption>(Application.ServiceProvider, this));
        return (TCommand)this;
    }

    public TCommand AddParameter(string name, object? defaultValue = default) {
        Children.Add(CreateInstance.Of<Parameter>(this, name, defaultValue));
        return (TCommand)this;
    }

    public TCommand AddParameter<TParameter>()
        where TParameter : Parameter<TParameter> {
        Children.Add(CreateInstance.Of<TParameter>(Application.ServiceProvider, this));
        return (TCommand)this;
    }

    public TCommand AddFlag(string name, params string[] aliases) {
        Children.Add(CreateInstance.Of<Flag>(this, name, aliases));
        return (TCommand)this;
    }

    public TCommand AddFlag<TFlag>()
        where TFlag : Flag<TFlag> {
        Children.Add(CreateInstance.Of<TFlag>(Application.ServiceProvider, this));
        return (TCommand)this;
    }
}
