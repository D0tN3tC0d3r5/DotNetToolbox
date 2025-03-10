namespace DotNetToolbox;

public static partial class Ensure {
    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? ItemsAreNotNull<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable {
        if (argument is null) return argument;
        (_, var list) = argument.Cast<object?>().Aggregate((index: 0, list: new List<Error>()), (result, item) => {
            if (item is not null) return result;
            result.list.Add(new(BuildMessage($"{paramName!}[{result.index}]", CannotBeNull), $"{paramName}[{result.index}]"));
            return (result.index + 1, result.list);
        });
        return list.Count == 0
            ? argument
            : throw new OperationFailureException(BuildMessage(paramName!, CannotContainNulls), list);
    }

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? ItemsAreValid<TArgument, TValue>(TArgument? argument, Func<TValue?, bool> isValid, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<TValue?> {
        if (argument is null) return argument;
        (_, var list) = argument.Aggregate((index: 0, list: new List<Error>()), (result, item) => {
            if (isValid(item)) return result;
            result.list.Add(new(BuildMessage($"{paramName!}[{result.index}]", IsInvalid),
                                $"{paramName}[{result.index}]"));
            return (result.index + 1, result.list);
        });
        return list.Count == 0
                   ? argument
                   : throw new OperationFailureException(BuildMessage(paramName!, CannotContainInvalidItem), list);
    }
}
