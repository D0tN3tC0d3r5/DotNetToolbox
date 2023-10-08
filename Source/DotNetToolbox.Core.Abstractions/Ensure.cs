using System.Results;
using System.Validation;

namespace System;

public static class Ensure {
    [return: NotNull]
    public static TArgument IsNotNull<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => argument
           ?? throw new ArgumentNullException(paramName, GetErrorMessage(CannotBeNull, paramName));

    public static TArgument HasValue<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : struct
        => argument
           ?? throw new ArgumentNullException(paramName, GetErrorMessage(CannotBeNull, paramName));

    [return: NotNull]
    public static TArgument IsOfType<TArgument>(object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => IsNotNull(argument, paramName) is not TArgument result
            ? throw new ArgumentException(string.Format(MustBeOfType, paramName, typeof(TArgument).Name, argument!.GetType().Name), paramName)
            : result;

    [return: NotNull]
    public static TArgument IsNotNullOrEmpty<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable
        => argument switch {
            null => throw new ArgumentException(GetErrorMessage(CannotBeNullOrEmpty, paramName), paramName),
            string { Length: 0 }
                => throw new ArgumentException(GetErrorMessage(CannotBeNullOrEmpty, paramName), paramName),
            string
                => argument,
            ICollection { Count: 0 }
                => throw new ArgumentException(GetErrorMessage(CannotBeNullOrEmpty, paramName), paramName),
            _ => argument,
        };

    public static string IsNotNullOrWhiteSpace(string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => argument is null
            ? throw new ArgumentException(GetErrorMessage(CannotBeNullOrWhiteSpace, paramName), paramName)
            : argument.Trim().Length == 0
                ? throw new ArgumentException(GetErrorMessage(CannotBeNullOrWhiteSpace, paramName), paramName)
                : argument;

    [return: NotNull]
    public static TArgument IsNotNullAndDoesNotContainNull<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable {
        argument = IsNotNull(argument, paramName);
        return argument switch {
            IEnumerable collection when collection.Cast<object?>().Any(item => item is null)
                => throw new ArgumentException(GetErrorMessage(CannotContainNull, paramName), paramName),
            _ => argument,
        };
    }

    [return: NotNull]
    [SuppressMessage("Style", "IDE0200:Remove unnecessary lambda expression", Justification = "<Pending>")]
    public static TArgument IsNotNullAndDoesNotContainNullOrEmpty<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<string?> {
        argument = IsNotNull(argument, paramName);
        return argument switch {
            // ReSharper disable once ConvertClosureToMethodGroup - it messes with code coverage
            IEnumerable<string?> collection when collection.Any(i => string.IsNullOrEmpty(i))
                => throw new ArgumentException(GetErrorMessage(CannotContainNullOrEmpty, paramName), paramName),
            _ => argument,
        };
    }

    [return: NotNull]
    [SuppressMessage("Style", "IDE0200:Remove unnecessary lambda expression", Justification = "<Pending>")]
    public static TArgument IsNotNullAndDoesNotContainNullOrWhiteSpace<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<string?> {
        argument = IsNotNull(argument, paramName);
        return argument switch {
            // ReSharper disable once ConvertClosureToMethodGroup - it messes with code coverage
            IEnumerable<string?> collection when collection.Any(i => string.IsNullOrWhiteSpace(i))
                => throw new ArgumentException(GetErrorMessage(CannotContainNullOrWhitespace, paramName), paramName),
            _ => argument,
        };
    }

    [return: NotNull]
    public static TArgument IsNotNullOrEmptyAndDoesNotContainNull<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable {
        argument = IsNotNullOrEmpty(argument, paramName);
        return argument switch {
            IEnumerable collection when collection.Cast<object?>().Any(x => x is null)
                => throw new ArgumentException(GetErrorMessage(CannotContainNull, paramName), paramName),
            _ => argument,
        };
    }

    [return: NotNull]
    public static TArgument IsNotNullOrEmptyAndDoesNotContainNullOrEmpty<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<string?> {
        argument = IsNotNullOrEmpty(argument, paramName);
        return argument switch {
            IEnumerable<string?> collection when collection.Any(string.IsNullOrEmpty)
                => throw new ArgumentException(GetErrorMessage(CannotContainNullOrEmpty, paramName), paramName),
            _ => argument,
        };
    }

    [return: NotNull]
    public static TArgument IsNotNullOrEmptyAndDoesNotContainNullOrWhiteSpace<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<string?> {
        argument = IsNotNullOrEmpty(argument, paramName);
        return argument switch {
            IEnumerable<string?> collection when collection.Any(string.IsNullOrWhiteSpace)
                => throw new ArgumentException(GetErrorMessage(CannotContainNullOrWhitespace, paramName), paramName),
            _ => argument,
        };
    }

    [return: NotNull]
    public static TItem ArgumentExistsAndIsOfType<TItem>(string methodName, IReadOnlyList<object?> arguments, uint argumentIndex, [CallerArgumentExpression(nameof(arguments))] string? paramName = null)
        => argumentIndex >= arguments.Count
            ? throw new ArgumentException($"Invalid number of arguments for '{methodName}'. Missing argument {argumentIndex}.", paramName)
            : arguments[(int)argumentIndex] is TItem value
                    ? value
                    : throw new ArgumentException($"Invalid type of {paramName}[{argumentIndex}] of '{methodName}'. Expected: {typeof(TItem).Name}. Found: {arguments[(int)argumentIndex]!.GetType().Name}.", $"{paramName}[{argumentIndex}]");

    public static TItem[] ArgumentsAreAllOfType<TItem>(string methodName, IReadOnlyList<object?> arguments, [CallerArgumentExpression(nameof(arguments))] string? paramName = null) {
        var list = IsNotNullOrEmptyAndDoesNotContainNull(arguments, paramName);
        for (var index = 0; index < list.Count; index++) {
            ArgumentExistsAndIsOfType<TItem>(methodName, arguments, (uint)index, paramName);
        }

        return list.Cast<TItem>().ToArray();
    }

    public static TItem? ArgumentExistsAndIsOfTypeOrDefault<TItem>(string methodName, IReadOnlyList<object?> arguments, uint argumentIndex, [CallerArgumentExpression(nameof(arguments))] string? paramName = null)
        => argumentIndex >= arguments.Count
            ? throw new ArgumentException($"Invalid number of arguments for '{methodName}'. Missing argument {argumentIndex}.", paramName)
            : arguments[(int)argumentIndex] switch {
                null => default,
                TItem value => value,
                _
                => throw new ArgumentException($"Invalid type of {paramName}[{argumentIndex}] of '{methodName}'. Expected: {typeof(TItem).Name}. Found: {arguments[(int)argumentIndex]!.GetType().Name}.", $"{paramName}[{argumentIndex}]"),
            };

    public static TItem?[] ArgumentsAreAllOfTypeOrDefault<TItem>(string methodName, IReadOnlyList<object?> arguments, [CallerArgumentExpression(nameof(arguments))] string? paramName = null) {
        var list = IsNotNullOrEmpty(arguments, paramName);
        for (var index = 0; index < list.Count; index++) {
            ArgumentExistsAndIsOfTypeOrDefault<TItem>(methodName, arguments, (uint)index, paramName);
        }

        return list.Select(i => i is null ? default : (TItem)i).ToArray();
    }
}
