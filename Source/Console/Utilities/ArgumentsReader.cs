namespace DotNetToolbox.ConsoleApplication.Utilities;

public static class ArgumentsReader {
    public static async Task<Result> Read(string[] arguments, IReadOnlyCollection<INode> children, CancellationToken ct) {
        var result = Success();
        for (var index = 0; index < arguments.Length; index++)
            (result, index) = await TrySetupChild(arguments, index, children, ct);
        return result;
    }

    private static async Task<(Result result, int index)> TrySetupChild(string[] arguments, int index, IReadOnlyCollection<INode> children, CancellationToken ct) {
        var token = arguments[index];
        var child = children.FirstOrDefault(arg => arg.Ids.Contains(token));
        var result = child switch {
            IFlag flag => await ReadFlag(flag, arguments[index], ct),
            IOption option when NoMoreTokens() => InvalidData($"Missing value for option '{option.Name}'."),
            IOption option => await ReadOption(option, arguments[++index], ct),
            ICommand or IAsyncCommand when NoMoreTokens() => await ReadCommand((IExecutable)child, [], ct),
            ICommand or IAsyncCommand => await ReadCommand((IExecutable)child, arguments[++index..], ct),
            ITrigger or IAsyncTrigger => await ReadAction((IExecutable)child, ct),
            _ when token.StartsWith("-") => InvalidData($"Unknown option: '{token}'."),
            _ => await ReadParameters(children, arguments, ct),
        };
        return (result, index);

        bool NoMoreTokens() => index >= arguments.Length - 1;
    }

    private static Task<Result> ReadFlag(IArgument flag, string argument, CancellationToken ct)
        => ReadArgument(flag, argument, ct);

    private static Task<Result> ReadOption(IArgument option, string argument, CancellationToken ct)
        => ReadArgument(option, argument, ct);

    private static async Task<Result> ReadArgument(IArgument argument, string value, CancellationToken ct) {
        var result = await argument.ClearData(ct);
        return result.IsSuccess
                   ? await argument.ReadData(value, ct)
                   : result;
    }

    private static Task<Result> ReadCommand(IExecutable command, string[] arguments, CancellationToken ct)
        => command.ExecuteAsync(arguments, ct);

    private static Task<Result> ReadAction(IExecutable action, CancellationToken ct)
        => action.ExecuteAsync([], ct);

    private static async Task<Result> ReadParameters(IEnumerable<INode> children, IReadOnlyList<string> arguments, CancellationToken ct) {
        var parameters = children.OfType<IParameter>().OrderBy(p => p.Order).ToArray();
        var index = 0;
        foreach (var parameter in parameters) {
            var result = await parameter.ClearData(ct);
            if (!result.IsSuccess) return result;
            if (index >= arguments.Count)
                return EnsureAllRequiredParametersAreSet(parameters);
            result = await parameter.ReadData(arguments[index], ct);
            if (!result.IsSuccess) return result;
            index++;
        }

        return Success();
    }

    private static Result EnsureAllRequiredParametersAreSet(IParameter[] parameters) {
        var missingParameters = parameters.Where(p => p is { IsRequired: true, IsSet: false }).Select(p => p.Name).ToArray();
        return missingParameters.Length > 0
                   ? Error($"Missing value for parameters '{string.Join("', '", missingParameters)}'.")
                   : Success();
    }
}
