namespace DotNetToolbox.ConsoleApplication.Nodes;

public abstract class NodeWithArguments<TCommand>(IHasChildren node, string name, params string[] aliases)
    : Node<TCommand>(node, name, aliases), IHasChildren
    where TCommand : NodeWithArguments<TCommand> {
    public ICollection<INode> Children { get; } = [];

    public IParameter[] Parameters => [.. Children.OfType<IParameter>().OrderBy(i => i.Order)];
    public IOption[] Options => [.. Children.OfType<IOption>()];
    public ICommand[] Commands => [.. Children.OfType<ICommand>().Except(Options.Cast<INode>()).Cast<ICommand>().OrderBy(i => i.Name)];

    public TCommand AddChildCommand<TChildCommand>()
        where TChildCommand : NodeWithArguments<TChildCommand> {
        Children.Add(CreateInstance.Of<TChildCommand>(Application.Services, this));
        return (TCommand)this;
    }

    public TCommand AddAction<TAction>()
        where TAction : Node<TAction> {
        Children.Add(CreateInstance.Of<TAction>(Application.Services, this));
        return (TCommand)this;
    }

    public TCommand AddOption(string name, params string[] aliases) {
        Children.Add(CreateInstance.Of<Option>(this, name, aliases));
        return (TCommand)this;
    }

    public TCommand AddOption<TOption>()
        where TOption : Option<TOption> {
        Children.Add(CreateInstance.Of<TOption>(Application.Services, this));
        return (TCommand)this;
    }

    public TCommand AddParameter(string name, object? defaultValue = default) {
        Children.Add(CreateInstance.Of<Parameter>(this, name, defaultValue));
        return (TCommand)this;
    }

    public TCommand AddParameter<TParameter>()
        where TParameter : Parameter<TParameter> {
        Children.Add(CreateInstance.Of<TParameter>(Application.Services, this));
        return (TCommand)this;
    }

    public TCommand AddFlag(string name, params string[] aliases) {
        Children.Add(CreateInstance.Of<Flag>(this, name, aliases));
        return (TCommand)this;
    }

    public TCommand AddFlag<TFlag>()
        where TFlag : Flag<TFlag> {
        Children.Add(CreateInstance.Of<TFlag>(Application.Services, this));
        return (TCommand)this;
    }
}
