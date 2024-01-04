namespace DotNetToolbox.ConsoleApplication.Nodes.Commands;

public abstract class Command<TCommand>
    : Executable<TCommand>
    , ICommand
    where TCommand : Command<TCommand> {

    protected Command(IHasChildren parent, string name, params string[] aliases)
        : base(parent, name, aliases) {
    }

    protected sealed override async Task<Result> ReadInput(string[] input, CancellationToken ct) {
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
            if (index >= input.Length && parameter.IsRequired)
                return Error($"Missing value for parameter {index + 1}:'{parameter.Name}'");
            if (index >= input.Length) break;
            var result = await parameter.SetValue(input[index], ct);
            if (!result.IsSuccess) return result;
            index++;
        }

        return Success();
    }
}
