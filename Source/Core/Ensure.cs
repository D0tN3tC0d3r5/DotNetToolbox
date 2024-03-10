namespace DotNetToolbox;

public static class Ensure {
    [return: NotNull]
    public static TArgument IsNotNull<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => argument ?? throw new ArgumentNullException(paramName, ValueCannotBeNull);

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
            ICollection { Count: 0 } => throw new ArgumentException(CollectionCannotBeEmpty, paramName),
            _ => argument,
        };

    [return: NotNull]
    public static TArgument IsNotNullOrEmpty<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable
        => argument switch {
            null => throw new ArgumentException(ValueCannotBeNull, paramName),
            string { Length: 0 } => throw new ArgumentException(StringCannotBeNullOrEmpty, paramName),
            ICollection { Count: 0 } => throw new ArgumentException(CollectionCannotBeEmpty, paramName),
            _ => argument,
        };

    public static string IsNotNullOrWhiteSpace(string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => argument is null || argument.Trim().Length == 0
            ? throw new ArgumentException(StringCannotBeNullOrWhiteSpace, paramName)
            : argument;

    public static TArgument IsValid<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IValidatable
        // null may be a valid value use IsNotNull to check nullability
        => IsValid(argument, arg => arg?.Validate() ?? Result.Success(), paramName)!;

    public static Task<TArgument?> IsValidAsync<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IValidatableAsync
        // null may be a valid value use IsNotNull to check nullability
        => IsValidAsync(argument, arg => arg?.Validate() ?? Result.SuccessTask(), paramName)!;

    [return: NotNullIfNotNull(nameof(validValue))]
    public static TArgument? IsValidOrDefault<TArgument>(TArgument? argument, TArgument? validValue)
        where TArgument : IValidatable {
        // null may be a valid value use IsNotNull to check nullability
        var result = argument?.Validate() ?? Result.Success();
        return result.IsSuccess && argument is not null
                   ? argument
                   : validValue;
    }

    public static async Task<TArgument> IsValidOrDefaultAsync<TArgument>(TArgument? argument, TArgument validValue)
        where TArgument : IValidatableAsync {
        // null may be a valid value use IsNotNull to check nullability
        var result = await (argument?.Validate() ?? Result.SuccessTask());
        return result.IsSuccess && argument is not null
                   ? argument
                   : validValue;
    }

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? IsValid<TArgument>(TArgument? argument, Func<TArgument, Result> validate, [CallerArgumentExpression(nameof(argument))]string? paramName = null) {
        // null may be a valid value use IsNotNull to check nullability
        if (argument is null) return argument;
        var result = validate(argument);
        return result.IsSuccess ? argument : throw new ValidationException(ValueIsInvalid, paramName!);
    }

    public static async Task<TArgument?> IsValidAsync<TArgument>(TArgument? argument, Func<TArgument, Task<Result>> validate, [CallerArgumentExpression(nameof(argument))] string? paramName = null) {
        // null may be a valid value use IsNotNull to check nullability
        if (argument is null) return argument;
        var result = await validate(argument);
        return result.IsSuccess
                   ? argument
                   : throw new ValidationException(ValueIsInvalid, paramName!);
    }

    [return: NotNullIfNotNull(nameof(defaultValue))]
    public static TArgument? IsValidOrDefault<TArgument>(TArgument? argument, Func<TArgument?, Result> validate, TArgument? defaultValue)
        => validate(argument).IsSuccess && argument is not null
               ? argument
               : defaultValue;

    [return: NotNullIfNotNull(nameof(defaultValue))]
    public static async Task<TArgument?> IsValidOrDefaultAsync<TArgument>(TArgument? argument, Func<TArgument?, Task<Result>> validate, TArgument? defaultValue)
        => (await validate(argument)).IsSuccess && argument is not null
               ? argument
               : defaultValue;

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? IsValid<TArgument>(TArgument? argument, Func<TArgument?, bool> isValid, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => isValid(argument)
               ? argument
               : throw new ValidationException(ValueIsInvalid, paramName!);


    public static async Task<TArgument?> IsValidAsync<TArgument>(TArgument? argument, Func<TArgument?, Task<bool>> isValid, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => await isValid(argument)
               ? argument
               : throw new ValidationException(ValueIsInvalid, paramName!);

    [return: NotNullIfNotNull(nameof(defaultValue))]
    public static TArgument? IsValidOrDefault<TArgument>(TArgument? argument, Func<TArgument?, bool> isValid, TArgument? defaultValue)
        => isValid(argument)
               ? argument ?? defaultValue
               : defaultValue;

    public static async Task<TArgument?> IsValidOrDefaultAsync<TArgument>(TArgument? argument, Func<TArgument?, Task<bool>> isValid, TArgument? defaultValue)
        => await isValid(argument) && argument is not null
               ? argument
               : defaultValue;

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? DoesNotContainNullItems<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable
        => argument?.Cast<object?>().All(i => i is not null) ?? true
               ? argument
               : throw new ValidationException(CollectionContainsNull, paramName!);

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? DoesNotContainInvalidItems<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<IValidatable>
        => argument?.All(i => i?.Validate().IsSuccess ?? true) ?? true
               ? argument
               : throw new ValidationException(CollectionContainsInvalid, paramName!);

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? DoesNotContainInvalidItems<TArgument, TValue>(TArgument? argument, Func<TValue?, Result> validate, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<TValue?>
        => argument?.All(i => validate(i).IsSuccess) ?? true
               ? argument
               : throw new ValidationException(CollectionContainsInvalid, paramName!);

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? DoesNotContainInvalidItems<TArgument, TValue>(TArgument? argument, Func<TValue?, bool> isValid, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<TValue?>
        => argument?.All(isValid) ?? true
                   ? argument
                   : throw new ValidationException(CollectionContainsInvalid, paramName!);
}
