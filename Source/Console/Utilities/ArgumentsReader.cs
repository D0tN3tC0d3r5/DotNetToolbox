namespace DotNetToolbox.ConsoleApplication.Utilities;

public static class ArgumentsReader {
    public static async Task<Result> Read(IHasChildren parent, IReadOnlyList<string> arguments, CancellationToken ct) {
        var result = Success();
        for (var index = 0; index < arguments.Count; index++)
            (result, index) = await TrySetupChild(parent, arguments, index, ct);
        return EnsureAllRequiredParametersAreSet(parent, result);
    }

    private static async Task<(Result result, int index)> TrySetupChild(IHasChildren parent, IReadOnlyList<string> arguments, int index, CancellationToken ct) {
        var child = FindChild(parent, arguments[index]);
        return child switch {
            IFlag => (await child.ExecuteAsync([], ct), index),
            IOption or ICommand => (await child.ExecuteAsync(arguments.Skip(++index).ToArray(), ct), index),
            _ => (await ReadParameters(parent, arguments.Skip(index).ToArray(), ct), arguments.Count - 1),
        };
    }

    private static INode? FindChild(IHasChildren parent, string token)
        => token.StartsWith('"')
               ? null
               : token.StartsWith('-')
                   ? token.StartsWith("--")
                         ? parent.Children.FirstOrDefault(c => c.Name.Equals(token.TrimStart('-'), StringComparison.CurrentCultureIgnoreCase))
                         : parent.Children.FirstOrDefault(c => c.Aliases.Contains(token.TrimStart('-')))
                   : parent.Children.FirstOrDefault(c => c.Name.Contains(token, StringComparison.CurrentCultureIgnoreCase)
                                               || c.Aliases.Contains(token));

    private static async Task<Result> ReadParameters(IHasChildren parent, IReadOnlyList<string> arguments, CancellationToken ct) {
        if (parent.Parameters.Length == 0) return Invalid($"Unknown argument '{arguments[0]}'. For a list of available arguments use '--help'.");
        var index = 0;
        var result = Success();
        foreach (var parameter in parent.Parameters) {
            if (index >= arguments.Count) break;
            result += arguments[index].StartsWith('-')
                ? Invalid($"Unknown argument '{arguments[index]}'. For a list of available arguments use '--help'.")
                : await parameter.ExecuteAsync([arguments[index]], ct);
            index++;
        }

        return result;
    }

    private static Result EnsureAllRequiredParametersAreSet(IHasChildren parent, Result result) {
        var missingParameters = parent.Parameters.Where(p => p is { IsRequired: true, IsSet: false }).Select(p => p.Name).ToArray();
        return missingParameters.Length > 0
                   ? Invalid($"Required parameter is missing: '{string.Join("', '", missingParameters)}'.")
                   : result;
    }
}
