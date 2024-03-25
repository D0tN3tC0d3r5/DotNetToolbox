namespace DotNetToolbox;

public static class Ensure {
    #region Null

    [return: NotNull]
    public static TArgument IsNotNull<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => argument ?? throw new ArgumentNullException(paramName, ValueCannotBeNull);

    [return: NotNull]
    public static TArgument GetDefaultIfNull<TArgument>(TArgument? argument, TArgument defaultValue)
        => argument ?? IsNotNull(defaultValue);

    #endregion

    #region Type

    [return: NotNull]
    public static TArgument IsOfType<TArgument>(object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => IsNotNull(argument, paramName) is not TArgument result
            ? throw new ArgumentException(string.Format(ValueMustBeOfType, typeof(TArgument).Name, argument!.GetType().Name), paramName)
            : result;

    #endregion

    #region Collection

    [return: NotNull]
    public static TArgument IsNotNullOrEmpty<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable
        => IsNotNull(argument, paramName) switch {
            string { Length: 0 } => throw new ArgumentException(StringCannotBeNullOrEmpty, paramName),
            ICollection { Count: 0 } => throw new ArgumentException(CollectionCannotBeEmpty, paramName),
            _ => argument!,
        };

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? IsNotEmpty<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable
        => argument switch {
            ICollection { Count: 0 } => throw new ArgumentException(CollectionCannotBeEmpty, paramName),
            not null when !argument.Cast<object>().Any() => throw new ArgumentException(CollectionCannotBeEmpty, paramName),
            _ => argument,
        };

    #endregion

    #region String

    public static string IsNotNullOrEmpty(string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => string.IsNullOrEmpty(argument)
               ? throw new ArgumentException(StringCannotBeNullOrEmpty, paramName)
               : argument;

    public static string IsNotNullOrWhiteSpace(string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => argument is null || argument.Trim().Length == 0
               ? throw new ArgumentException(StringCannotBeNullOrWhiteSpace, paramName)
               : argument;

    [return: NotNullIfNotNull(nameof(defaultValue))]
    public static string? GetDefaultIfNullOrEmpty(string? argument, string? defaultValue)
        => string.IsNullOrEmpty(argument)
               ? defaultValue
               : argument;

    [return: NotNullIfNotNull(nameof(defaultValue))]
    public static string? GetDefaultIfNullOrWhiteSpace(string? argument, string? defaultValue)
        => string.IsNullOrWhiteSpace(argument)
               ? defaultValue
               : argument;

    #endregion

    #region IsValid

    [return: NotNull]
    public static TArgument IsValid<TArgument>(TArgument? argument, IDictionary<string, object?>? context = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IValidatable
        => IsValid(IsNotNull(argument, paramName), arg => arg?.Validate(context) ?? Result.Success(), paramName)!;

    [return: NotNullIfNotNull(nameof(defaultValue))]
    public static TArgument? GetDefaultIfInvalid<TArgument>(TArgument? argument, IDictionary<string, object?>? context = null, TArgument? defaultValue = default)
        where TArgument : IValidatable {
        var result = argument?.Validate(context) ?? Result.Success();
        return result.IsSuccess && argument is not null
                   ? argument
                   : defaultValue;
    }

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? IsValid<TArgument>(TArgument? argument, Func<TArgument, Result> validate, [CallerArgumentExpression(nameof(argument))] string? paramName = null) {
        var result = validate(IsNotNull(argument, paramName));
        return result.IsSuccess ? argument : throw new ValidationException(ValueIsInvalid, paramName!);
    }

    [return: NotNullIfNotNull(nameof(defaultValue))]
    public static TArgument? GetDefaultIfInvalid<TArgument>(TArgument? argument, Func<TArgument?, Result> validate, TArgument? defaultValue = default)
        => validate(argument).IsSuccess && argument is not null
               ? argument
               : defaultValue;

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? IsValid<TArgument>(TArgument? argument, Func<TArgument?, bool> isValid, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => isValid(IsNotNull(argument, paramName))
               ? argument
               : throw new ValidationException(ValueIsInvalid, paramName!);

    [return: NotNullIfNotNull(nameof(defaultValue))]
    public static TArgument? GetDefaultIfInvalid<TArgument>(TArgument? argument, Func<TArgument?, bool> isValid, TArgument? defaultValue = default)
        => isValid(argument)
               ? argument ?? defaultValue
               : defaultValue;

    #endregion

    #region Items

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? ItemsAreNotNull<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable
        => argument?.Cast<object?>().All(i => i is not null) ?? true
               ? argument
               : throw new ValidationException(CollectionContainsNull, paramName!);

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? ItemsAreValid<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<IValidatable>
        => argument?.All(i => i?.Validate().IsSuccess ?? true) ?? true
               ? argument
               : throw new ValidationException(CollectionContainsInvalid, paramName!);

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? ItemsAreValid<TArgument, TValue>(TArgument? argument, Func<TValue?, Result> validate, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<TValue?>
        => argument?.All(i => validate(i).IsSuccess) ?? true
               ? argument
               : throw new ValidationException(CollectionContainsInvalid, paramName!);

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? ItemsAreValid<TArgument, TValue>(TArgument? argument, Func<TValue?, bool> isValid, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable<TValue?>
        => argument?.All(isValid) ?? true
                   ? argument
                   : throw new ValidationException(CollectionContainsInvalid, paramName!);

    #endregion

    #region Async

    public static Task<TArgument?> IsValidAsync<TArgument>(TArgument? argument, IDictionary<string, object?>? context = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IValidatableAsync
        => IsValidAsync(IsNotNull(argument, paramName), arg => arg?.Validate(context) ?? Result.SuccessTask(), paramName)!;

    public static async Task<TArgument?> GetDefaultIfIsNotValidAsync<TArgument>(TArgument? argument, IDictionary<string, object?>? context = null, TArgument? defaultValue = default)
        where TArgument : IValidatableAsync {
        var result = await (argument?.Validate(context) ?? Result.SuccessTask());
        return result.IsSuccess && argument is not null
                   ? argument
                   : defaultValue;
    }

    public static async Task<TArgument?> IsValidAsync<TArgument>(TArgument? argument, Func<TArgument, Task<Result>> validate, [CallerArgumentExpression(nameof(argument))] string? paramName = null) {
        var result = await validate(IsNotNull(argument, paramName));
        return result.IsSuccess
                   ? argument
                   : throw new ValidationException(ValueIsInvalid, paramName!);
    }

    public static async Task<TArgument?> GetDefaultIfIsNotValidAsync<TArgument>(TArgument? argument, Func<TArgument?, Task<Result>> validate, TArgument? defaultValue = default)
        => (await validate(argument)).IsSuccess && argument is not null
               ? argument
               : defaultValue;

    public static async Task<TArgument?> IsValidAsync<TArgument>(TArgument? argument, Func<TArgument?, Task<bool>> isValid, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => await isValid(IsNotNull(argument, paramName))
               ? argument
               : throw new ValidationException(ValueIsInvalid, paramName!);

    public static async Task<TArgument?> GetDefaultIfIsNotValidAsync<TArgument>(TArgument? argument, Func<TArgument?, Task<bool>> isValid, TArgument? defaultValue = default)
        => await isValid(argument) && argument is not null
               ? argument
               : defaultValue;

    #endregion
}
