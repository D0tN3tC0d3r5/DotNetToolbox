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
                   : throw GenerateException(paramName, invalidItems, ElementAtCannotBeNull);
    }

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? AllAreNotNullOrEmpty<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<string?> {
        var invalidItems = GetIndexedItems<TArgument, string?>(argument).Where(i => string.IsNullOrEmpty(i.Value)).Select(i => i.Index).ToArray();
        return invalidItems.Length == 0
                   ? argument
                   : throw GenerateException(paramName, invalidItems, ElementAtCannotBeNullOrEmpty);
    }

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? AllAreNotNullOrWhiteSpace<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<string?> {
        var invalidItems = GetIndexedItems<TArgument, string?>(argument).Where(i => string.IsNullOrWhiteSpace(i.Value)).Select(i => i.Index).ToArray();
        return invalidItems.Length == 0
               ? argument
               : throw GenerateException(paramName, invalidItems, ElementAtCannotBeNullOrWhiteSpace);
    }

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
        return result.IsSuccess
                   ? value
                   : @default;
    }

    public static TArgument? IsValid<TArgument>(TArgument? argument, Func<TArgument?, Result> validate, [CallerArgumentExpression(nameof(argument))] string? paramName = null) {
        try {
            validate(argument).EnsureIsSuccess();
            return argument;
        }
        catch (Exception ex) {
            throw new ArgumentException(ex.Message, paramName, ex);
        }
    }

    public static TArgument? IsValidOrDefault<TArgument>(TArgument? argument, Func<TArgument?, Result> validate, TArgument? @default)
        => validate(argument).IsSuccess
               ? argument
               : @default;

    public static TArgument? IsValid<TArgument>(TArgument? argument, Func<TArgument?, bool> isValid, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => isValid(argument)
               ? argument
               : throw new ArgumentException(ValueIsNotValid, paramName!);

    public static TArgument? IsValidOrDefault<TArgument>(TArgument? argument, Func<TArgument?, bool> isValid, TArgument? @default)
        => isValid(argument)
               ? argument
               : @default;

    public static TArgument? AllAreValid<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<IValidatable> {
        var invalidItems = GetIndexedItems<TArgument, IValidatable>(argument)
                          .Where(i => i.Value?.Validate().IsSuccess ?? true)
                          .Select(i => i.Index)
                          .ToArray();
        return invalidItems.Length == 0
                   ? argument
                   : throw GenerateException(paramName, invalidItems, ElementAtCannotBeNullOrWhiteSpace);
    }

    public static TArgument? AllAreValid<TArgument, TValue>(TArgument? argument, Func<TValue?, Result> validate, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<TValue> {
        var invalidItems = GetIndexedItems<TArgument, TValue>(argument)
                          .Where(i => !validate(i.Value).IsSuccess)
                          .Select(i => i.Index)
                          .ToArray();
        return invalidItems.Length == 0
                   ? argument
                   : throw GenerateException(paramName, invalidItems, ElementAtCannotBeNullOrWhiteSpace);
    }

    public static TArgument? AllAreValid<TArgument, TValue>(TArgument? argument, Func<TValue?, bool> isValid, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<TValue> {
        var invalidItems = GetIndexedItems<TArgument, TValue>(argument)
                          .Where(i => !isValid(i.Value))
                          .Select(i => i.Index)
                          .ToArray();
        return invalidItems.Length == 0
                   ? argument
                   : throw GenerateException(paramName, invalidItems, ElementAtCannotBeNullOrWhiteSpace);
    }

    private static IEnumerable<Indexed<TValue>> GetIndexedItems<TArgument, TValue>(TArgument? argument)
        where TArgument : IEnumerable
        => (argument?.Cast<TValue?>() ?? Enumerable.Empty<TValue?>()).Select((x, i) => new Indexed<TValue>(i, x)).ToArray();

    private static ArgumentException GenerateException(string? paramName, IEnumerable<int> emptyElements, string message) {
        var errors = emptyElements.Select(i => new ValidationError(string.Format(message, i)));
        return new(CollectionIsInvalid, paramName, new ValidationException(errors));
    }
}
