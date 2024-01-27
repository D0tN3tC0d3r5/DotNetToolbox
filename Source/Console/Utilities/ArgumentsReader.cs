namespace DotNetToolbox.ConsoleApplication.Utilities;

public static class ArgumentsReader {
    public static async Task<Result> Read(IReadOnlyList<string> arguments, IReadOnlyCollection<INode> children, CancellationToken ct) {
        var result = Success();
        for (var index = 0; index < arguments.Count; index++)
            (result, index) = await TrySetupChild(arguments, index, children, ct);
        return result;
    }

    private static async Task<(Result result, int index)> TrySetupChild(IReadOnlyList<string> arguments, int index, IReadOnlyCollection<INode> children, CancellationToken ct) {
        var token = arguments[index];
        var child = children.FirstOrDefault(arg => arg.Ids.Contains(token));
        var parameters = children.OfType<IParameter>().OrderBy(p => p.Order).ToArray();
        var result = child switch {
            IFlag flag => await flag.ExecuteAsync(ct),
            IOption option when NoMoreTokens() => Invalid($"Missing value for option '{token}'."),
            IOption option => await option.ExecuteAsync(arguments.Skip(++index).ToArray(), ct),
            ICommand when NoMoreTokens() => await child.ExecuteAsync(ct),
            ICommand => await child.ExecuteAsync(arguments.Skip(++index).ToArray(), ct),
            _ when token.StartsWith('-') => Invalid($"Unknown argument '{token}'. For a list of arguments use '--help'."),
            _ when parameters.Length == 0 => Invalid($"Unknown argument '{token}'. For a list of arguments use '--help'."),
            _ => await ReadParameters(parameters, arguments, ct),
        };
        return (result, index);

        bool NoMoreTokens() => index >= arguments.Count - 1;
    }

    private static async Task<Result> ReadParameters(IReadOnlyCollection<IParameter> parameters, IReadOnlyList<string> arguments, CancellationToken ct) {
        var index = 0;
        var result = Success();
        foreach (var parameter in parameters) {
            if (index >= arguments.Count)
                return EnsureAllRequiredParametersAreSet(parameters);
            result += await parameter.ExecuteAsync(arguments, ct);
            index++;
        }

        return result;
    }

    private static Result EnsureAllRequiredParametersAreSet(IEnumerable<IParameter> parameters) {
        var missingParameters = parameters.Where(p => p is { IsRequired: true, IsSet: false }).Select(p => p.Name).ToArray();
        return missingParameters.Length > 0
                   ? Error($"Missing value for parameters '{string.Join("', '", missingParameters)}'.")
                   : Success();
    }
}
