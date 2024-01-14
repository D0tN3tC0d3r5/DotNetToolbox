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
        var elements = (argument?.Cast<object?>() ?? Enumerable.Empty<object?>()).Select((x, i) => new Indexed(i, x)).ToArray();
        var invalidItems = elements.Where(i => i.Value is null).ToArray();
        return invalidItems.Length == 0
                   ? argument
                   : throw GenerateException(paramName, invalidItems, ElementAtCannotBeNull);
    }

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? AllAreNotNullOrEmpty<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<string?> {
        var elements = (argument ?? Enumerable.Empty<string?>()).Select((x, i) => new Indexed<string>(i, x)).ToArray();
        var invalidItems = elements.Where(i => string.IsNullOrEmpty(i.Value)).ToArray();
        return invalidItems.Length == 0
                   ? argument
                   : throw GenerateException(paramName, invalidItems, ElementAtCannotBeNullOrEmpty);
    }

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? AllAreNotNullOrWhiteSpace<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<string?> {
        var elements = (argument ?? Enumerable.Empty<string?>()).Select((x, i) => new Indexed(i, x)).ToArray();
        var invalidItems = elements.Where(i => string.IsNullOrWhiteSpace((string?)i.Value)).ToArray();
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
        return result.IsSuccess ?  value : @default;
    }

    public static TArgument? IsValid<TArgument>(TArgument? argument, Func<TArgument?, Result> validate, [CallerArgumentExpression(nameof(argument))] string? paramName = null) {
        validate(argument).EnsureIsSuccess();
        return argument;
    }

    public static TArgument? IsValidOrDefault<TArgument>(TArgument? argument, Func<TArgument?, Result> validate, TArgument? @default) {
        var result = validate(argument);
        return result switch {
            { HasException: true } => throw result.Errors.First().Exception!,
            { HasErrors: true } => @default,
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

    public static TArgument? AllAreValid<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<IValidatable> {
        var elements = (argument ?? Enumerable.Empty<IValidatable>()).Select((x, i) => new Indexed<IValidatable>(i, x)).ToArray();
        var invalidItems = GetIndexedArguments<TArgument, IValidatable>(argument)
                          .Where(i => i.Value?.Validate().IsSuccess ?? true)
                          .Select(i => i.Index)
                          .ToArray();
        //var invalidItems = elements.Where(i => !(i.Value?.Validate().IsSuccess ?? true)).ToArray();
        return invalidItems.Length == 0
                   ? argument
                   : throw GenerateException(paramName, invalidItems, ElementAtCannotBeNullOrWhiteSpace);
    }

    public static TArgument? AllAreValid<TArgument>(TArgument? argument, Func<object?, Result> validate, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable {
        var invalidItems = GetIndexedArguments<TArgument, object>(argument)
                          .Where(i => !validate(i.Value).IsSuccess)
                          .Select(i => i.Index)
                          .ToArray();
        return invalidItems.Length == 0
                   ? argument
                   : throw GenerateException(paramName, invalidItems, ElementAtCannotBeNullOrWhiteSpace);
    }

    public static TArgument? AllAreValid<TArgument>(TArgument? argument, Func<object?, bool> isValid, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable {
        var invalidItems = GetIndexedArguments<TArgument, object>(argument)
                          .Where(i => !isValid(i.Value))
                          .Select(i => i.Index)
                          .ToArray();
        return invalidItems.Length == 0
                   ? argument
                   : throw GenerateException(paramName, invalidItems, ElementAtCannotBeNullOrWhiteSpace);
    }

    private static IEnumerable<Indexed<TValue>> GetIndexedArguments<TArgument, TValue>(TArgument? argument)
        where TArgument : IEnumerable
        => (argument?.Cast<TValue?>() ?? Enumerable.Empty<TValue?>()).Select((x, i) => new Indexed<TValue>(i, x)).ToArray();

    private static ArgumentException GenerateException(string? paramName, IEnumerable<int> emptyElements, string message) {
        var errors = emptyElements.Select(i => new ValidationError(string.Format(message, i)));
        return new(CollectionIsInvalid, paramName, new ValidationException(errors));
    }
}
