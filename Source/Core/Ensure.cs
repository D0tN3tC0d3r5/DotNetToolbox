namespace DotNetToolbox;

public static class Ensure {
    [return: NotNull]
    public static TArgument IsNotNull<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => argument ?? throw new ArgumentNullException(paramName, string.Format(ValueCannotBeNull, paramName));

    [return: NotNull]
    public static TArgument IsNotNullOrDefault<TArgument>(TArgument? argument, TArgument defaultValue)
        => argument ?? IsNotNull(defaultValue);

    [return: NotNull]
    public static TArgument IsOfType<TArgument>(object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => IsNotNull(argument, paramName) is not TArgument result
            ? throw new ArgumentException(string.Format(ValueMustBeOfType, typeof(TArgument).Name, argument!.GetType().Name), paramName)
            : result;

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? IsNotEmpty<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable
        => argument switch {
            ICollection { Count: 0 } => throw new ArgumentException(string.Format(CollectionCannotBeEmpty, paramName), paramName),
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
    public static TArgument? AllAreNotNull<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable {
        var invalidItems = GetIndexedItems<TArgument, object?>(argument).Where(i => i.Value is null).Select(i => i.Index).ToArray();
        return invalidItems.Length == 0
                   ? argument
                   : throw GenerateException(paramName!, invalidItems, ElementAtCannotBeNull);
    }

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? AllAreNotNullOrEmpty<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<string?> {
        var invalidItems = GetIndexedItems<TArgument, string?>(argument).Where(i => string.IsNullOrEmpty(i.Value)).Select(i => i.Index).ToArray();
        return invalidItems.Length == 0
                   ? argument
                   : throw GenerateException(paramName!, invalidItems, ElementAtCannotBeNullOrEmpty);
    }

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? AllAreNotNullOrWhiteSpace<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<string?> {
        var invalidItems = GetIndexedItems<TArgument, string?>(argument).Where(i => string.IsNullOrWhiteSpace(i.Value)).Select(i => i.Index).ToArray();
        return invalidItems.Length == 0
               ? argument
               : throw GenerateException(paramName!, invalidItems, ElementAtCannotBeNullOrWhiteSpace);
    }

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument IsValid<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IValidatable
        => IsValid(argument, arg => IsNotNull(arg).Validate(), paramName)!;

    [return: NotNullIfNotNull(nameof(validValue))]
    public static TArgument? IsValidOrDefault<TArgument>(TArgument? argument, TArgument? validValue)
        where TArgument : IValidatable { // null maybe a valid value
        var result = argument?.Validate() ?? Result.Success();
        return result.IsSuccess
                   ? argument ?? validValue
                   : validValue;
    }

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? IsValid<TArgument>(TArgument? argument, Func<TArgument?, Result> validate, [CallerArgumentExpression(nameof(argument))] string? paramName = null) {
        validate(argument).EnsureIsSuccess(null, paramName);
        return argument;
    }

    [return: NotNullIfNotNull(nameof(defaultValue))]
    public static TArgument? IsValidOrDefault<TArgument>(TArgument? argument, Func<TArgument?, Result> validate, TArgument? defaultValue)
        => validate(argument).IsSuccess
               ? argument ?? defaultValue
               : defaultValue;

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? IsValid<TArgument>(TArgument? argument, Func<TArgument?, bool> isValid, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => isValid(argument)
               ? argument
               : throw new ValidationException(ValueIsNotValid, paramName!);

    [return: NotNullIfNotNull(nameof(defaultValue))]
    public static TArgument? IsValidOrDefault<TArgument>(TArgument? argument, Func<TArgument?, bool> isValid, TArgument? defaultValue)
        => isValid(argument)
               ? argument ?? defaultValue
               : defaultValue;

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? AllAreValid<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<IValidatable> {
        var invalidItems = GetIndexedItems<TArgument, IValidatable>(argument)
                          .Where(i => !i.Value?.Validate().IsSuccess ?? false)
                          .Select(i => i.Index)
                          .ToArray();
        return invalidItems.Length == 0
                   ? argument
                   : throw GenerateException(paramName!, invalidItems, ElementAtCannotBeNullOrWhiteSpace);
    }

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? AllAreValid<TArgument, TValue>(TArgument? argument, Func<TValue?, Result> validate, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<TValue> {
        var invalidItems = GetIndexedItems<TArgument, TValue>(argument)
                          .Where(i => !validate(i.Value).IsSuccess)
                          .Select(i => i.Index)
                          .ToArray();
        return invalidItems.Length == 0
                   ? argument
                   : throw GenerateException(paramName!, invalidItems, ElementAtCannotBeNullOrWhiteSpace);
    }

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? AllAreValid<TArgument, TValue>(TArgument? argument, Func<TValue?, bool> isValid, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<TValue> {
        var invalidItems = GetIndexedItems<TArgument, TValue>(argument)
                          .Where(i => !isValid(i.Value))
                          .Select(i => i.Index)
                          .ToArray();
        return invalidItems.Length == 0
                   ? argument
                   : throw GenerateException(paramName!, invalidItems, ElementAtCannotBeNullOrWhiteSpace);
    }

    private static Indexed<TValue>[] GetIndexedItems<TArgument, TValue>(TArgument? argument)
        where TArgument : IEnumerable
        => (argument?.Cast<TValue?>() ?? []).Select((x, i) => new Indexed<TValue>(i, x)).ToArray();

    private static ValidationException GenerateException(string paramName, IEnumerable<int> emptyElements, string message) {
        var errors = emptyElements.Select(i => new ValidationError(string.Format(message, i))).ToArray();
        return new(CollectionIsInvalid, paramName, errors);
    }
}
