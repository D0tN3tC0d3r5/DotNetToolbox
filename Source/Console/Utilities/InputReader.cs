namespace ConsoleApplication.Utilities;

public static class InputReader {
    public static async Task<Result> ParseTokens(INode[] children, string[] tokens, CancellationToken ct) {
        var result = Success();
        for (var index = 0; index < tokens.Length; index++) {
            var token = tokens[index];
            var child = children.FirstOrDefault(arg => arg.Ids.Contains(token));
            switch (child) {
                case IFlag flag:
                    result = await ReadArgument(flag, tokens[index], ct);
                    break;
                case IOption option:
                    index++;
                    var hasNextToken = index >= tokens.Length || tokens[index].StartsWith('-');
                    result = hasNextToken
                                 ? Error($"Missing value for option '{token}'.")
                                 : await ReadArgument(option, tokens[index], ct);
                    break;
                case IExecutable executable:
                    index++;
                    var arguments = index >= tokens.Length ? [] : tokens[index..];
                    return await executable.ExecuteAsync(arguments, ct);
                default:
                    return await ProcessParameters(children, tokens, ct);
            }
            if (!result.IsSuccess) break;
        }
        return result;
    }

    private static async Task<Result> ReadArgument(IArgument argument, string token, CancellationToken ct) {
        var result = await argument.ClearData(ct);
        return result.IsSuccess
                   ? await argument.ReadData(token, ct)
                   : result;
    }

    private static async Task<Result> ProcessParameters(IEnumerable<INode> children, IReadOnlyList<string> tokens, CancellationToken ct) {
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
