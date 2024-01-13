namespace DotNetToolbox.ConsoleApplication.Utilities;

public static class InputReader {
    public static async Task<Result> ParseTokens(INode[] children, string[] tokens, CancellationToken ct) {
        var result = Success();
        for (var index = 0; index < tokens.Length; index++)
            (result, index) = await ParseToken(index, tokens, children, ct);
        return result;
    }

    private static async Task<(Result result, int index)> ParseToken(int index, string[] tokens, INode[] children, CancellationToken ct) {
        var token = tokens[index];
        var child = children.FirstOrDefault(arg => arg.Ids.Contains(token));
        var result = child switch {
            IFlag flag => await ReadArgument(flag, tokens[index], ct),
            IOption option => await ReadOption(option, tokens, index++, ct),
            ICommand command => await ReadCommand(command, tokens, index++, ct),
            IAction action => await ReadAction(action, ct),
            _ => await ReadParameters(children, tokens, ct),
        };
        return (result, index);
    }

    private static async Task<Result> ReadArgument(IArgument argument, string token, CancellationToken ct) {
        var result = await argument.ClearData(ct);
        return result.IsSuccess
                   ? await argument.ReadData(token, ct)
                   : result;
    }

    private static async Task<Result> ReadOption(IArgument option, string[] tokens, int index, CancellationToken ct) {
        var value = index >= tokens.Length || tokens[index].StartsWith('-') ? default : tokens[index];
        var result = value is null
                         ? Error($"Missing value for option '{option.Name}'.")
                         : await ReadArgument(option, tokens[index], ct);
        return result;
    }

    private static async Task<Result> ReadCommand(IExecutable command, string[] tokens, int index, CancellationToken ct) {
        var args = index >= tokens.Length ? [] : tokens[index..];
        var result = await command.ExecuteAsync(args, ct);
        return result;
    }

    private static async Task<Result> ReadAction(IExecutable action, CancellationToken ct) {
        var result = await action.ExecuteAsync([], ct);
        return result;
    }

    private static async Task<Result> ReadParameters(IEnumerable<INode> children, IReadOnlyList<string> tokens, CancellationToken ct) {
        var parameters = children.OfType<IParameter>().OrderBy(p => p.Order).ToArray();
        var index = 0;
        foreach (var parameter in parameters) {
            var result = await parameter.ClearData(ct);
            if (!result.IsSuccess) return result;
            if (index >= tokens.Count)
                return EnsureAllRequiredParametersAreSet(parameters);
            result = await parameter.ReadData(tokens[index], ct);
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
