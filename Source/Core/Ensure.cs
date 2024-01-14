namespace DotNetToolbox;

public static class Ensure {
    [return: NotNull]
    public static TArgument IsNotNull<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => argument ?? throw new ArgumentNullException(paramName, string.Format(ValueCannotBeNull, paramName));

    [return: NotNull]
    public static TArgument IsNotNullOrDefault<TArgument, TDefault>(TArgument? argument, TDefault @default)
        where TDefault : notnull, TArgument
        => argument ?? @default;

    [return: NotNull]
    public static TArgument IsOfType<TArgument>(object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => IsNotNull(argument, paramName) is not TArgument result
            ? throw new ArgumentException(string.Format(ValueMustBeOfType, typeof(TArgument).Name, argument!.GetType().Name), paramName)
            : result;

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? IsNotEmpty<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable
        => argument switch {
            ICollection { Count: 0 } => throw new ArgumentException(string.Format(ValueCannotBeEmpty, paramName), paramName),
            _ => argument,
        };

    [return: NotNull]
    public static TArgument IsNotNullOrEmpty<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable
        => argument switch {
            null => throw new ArgumentException(string.Format(ValueCannotBeNullOrEmpty, paramName), paramName),
            string { Length: 0 } => throw new ArgumentException(string.Format(ValueCannotBeNullOrEmpty, paramName), paramName),
            ICollection { Count: 0 } => throw new ArgumentException(string.Format(ValueCannotBeNullOrEmpty, paramName), paramName),
            _ => argument,
        };

    public static string IsNotNullOrWhiteSpace(string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => argument is null || argument.Trim().Length == 0
            ? throw new ArgumentException(string.Format(ValueCannotBeNullOrWhiteSpace, paramName), paramName)
            : argument;

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? HasNoNull<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable
        => argument switch {
            IEnumerable collection when collection.Cast<object?>().Any(item => item is null)
                => throw new ArgumentException(string.Format(ValueCannotContainNullItem, paramName), paramName),
            _ => argument,
        };

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? HasNoNullOrEmpty<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<string?>
        => argument switch {
            // ReSharper disable once ConvertClosureToMethodGroup - it messes with code coverage
            IEnumerable<string?> collection when collection.Any(string.IsNullOrEmpty)
                => throw new ArgumentException(string.Format(ValueCannotContainEmptyString, paramName), paramName),
            _ => argument,
        };

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? HasNoNullOrWhiteSpace<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<string?>
        => argument switch {
            // ReSharper disable once ConvertClosureToMethodGroup - it messes with code coverage
            IEnumerable<string?> collection when collection.Any(string.IsNullOrWhiteSpace)
                => throw new ArgumentException(string.Format(ValueCannotContainWhiteSpaceString, paramName), paramName),
            _ => argument,
        };

    [return: NotNull]
    public static TArgument IsValid<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IValidatable
        => IsValid(argument, arg => IsNotNull(arg).Validate(), paramName)!;

    [return: NotNull]
    public static TArgument IsValidOrDefault<TArgument, TDefault>(TArgument? argument, TDefault @default)
        where TArgument : IValidatable
        where TDefault : TArgument {
        var value = IsNotNull(argument ?? @default);
        var result = value.Validate();
        return result switch { { HasException: true } => throw result.Exception, { HasErrors: true } => @default,
            _ => value,
        };
    }

    public static TArgument? IsValid<TArgument>(TArgument? argument, Func<TArgument?, Result> validate, [CallerArgumentExpression(nameof(argument))] string? paramName = null) {
        validate(argument).EnsureIsSuccess();
        return argument;
    }

    public static TArgument? IsValidOrDefault<TArgument>(TArgument? argument, Func<TArgument?, Result> validate, TArgument? @default) {
        var result = validate(argument);
        return result switch { { HasException: true } => throw result.Exception, { HasErrors: true } => @default,
            _ => argument,
        };
    }

    public static TArgument? IsValid<TArgument>(TArgument? argument, Func<TArgument?, bool> isValid, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => isValid(argument)
               ? argument
               : throw new ArgumentException(ValueIsNotValid, paramName!);

    public static TArgument? IsValidOrDefault<TArgument>(TArgument? argument, Func<TArgument?, bool> isValid, TArgument? @default)
        => isValid(argument)
               ? argument
               : @default;
}
