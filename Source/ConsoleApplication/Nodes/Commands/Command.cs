namespace DotNetToolbox.ConsoleApplication.Nodes.Commands;

public abstract class Command<TCommand>
    : Executable<TCommand>
    , ICommand
    where TCommand : Command<TCommand> {

    protected Command(IHasChildren node, string name, params string[] aliases)
        : base(node, name, aliases) {
        AddAction<HelpAction>();
    }

    public TCommand AddCommand<TChildCommand>()
        where TChildCommand : Command<TChildCommand> {
        Children.Add(CreateInstance.Of<TChildCommand>(Application.ServiceProvider, this));
        return (TCommand)this;
    }

    public TCommand AddAction<TAction>()
        where TAction : Arguments.Action<TAction> {
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

    protected sealed override async Task<Result> ReadArguments(string[] input, CancellationToken ct) {
        for (var index = 0; index < input.Length; index++) {
            var id = input[index];
            var argument = Children.FirstOrDefault(arg => arg.Ids.Contains(id));
            switch (argument) {
                case IHasValue hasValue:
                    if (argument is IOption) index++;
                    if (index >= input.Length) return Error($"Missing value for option '{id}'");
                    var argumentResult = await hasValue.SetValue(input[index], ct);
                    if (!argumentResult.IsSuccess) return argumentResult;
                    break;
                case IExecutable executable:
                    index++;
                    var arguments = index >= input.Length ? [] : input[index..];
                    var actionResult = await executable.ExecuteAsync(arguments, ct);
                    if (!actionResult.IsSuccess) return actionResult;
                    break;
                default:
                    return await ProcessParameters(input, ct);
            }
        }
        return Success();
    }

    private async Task<Result> ProcessParameters(string[] input, CancellationToken ct) {
        var parameters = Children.OfType<IParameter>().OrderBy(p => p.Order).ToArray();
        var index = 0;
        foreach (var parameter in parameters) {
            if (index >= input.Length && parameter.DefaultValue is null)
                return Error($"Missing value for parameter {index + 1}:'{parameter.Name}'");
            if (index >= input.Length) break;
            var result = await parameter.SetValue(input[index], ct);
            if (!result.IsSuccess) return result;
            index++;
        }

        return Success();
    }
}
