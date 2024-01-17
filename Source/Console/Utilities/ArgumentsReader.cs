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
        var child = children.OfType<IArgument>().FirstOrDefault(arg => arg.Ids.Contains(token));
        var result = child switch {
            IFlag flag => await flag.SetValue(ct),
            IOption option when NoMoreTokens() => Invalid($"Missing value for option '{option.Name}'."),
            IOption option => await option.SetValue(arguments[++index], ct),
            ICommand or IAsyncCommand when NoMoreTokens() => await ((IExecutable)child).ExecuteAsync([], ct),
            ICommand or IAsyncCommand => await ((IExecutable)child).ExecuteAsync(arguments[++index..], ct),
            _ when token.StartsWith('-') => Invalid($"Unknown option: '{token}'."),
            _ => await ReadParameters(children, arguments, ct),
        };
        return (result, index);

        bool NoMoreTokens() => index >= arguments.Length - 1;
    }

    private static async Task<Result> ReadParameters(IEnumerable<INode> children, IReadOnlyList<string> arguments, CancellationToken ct) {
        var parameters = children.OfType<IParameter>().OrderBy(p => p.Order).ToArray();
        var index = 0;
        var result = Success();
        foreach (var parameter in parameters) {
            if (index >= arguments.Count)
                return EnsureAllRequiredParametersAreSet(parameters);
            result += await parameter.SetValue(arguments[index], ct);
            index++;
        }

        return result;
    }

    private static Result EnsureAllRequiredParametersAreSet(IParameter[] parameters) {
        var missingParameters = parameters.Where(p => p is { IsRequired: true, IsSet: false }).Select(p => p.Name).ToArray();
        return missingParameters.Length > 0
                   ? Error($"Missing value for parameters '{string.Join("', '", missingParameters)}'.")
                   : Success();
    }
}
