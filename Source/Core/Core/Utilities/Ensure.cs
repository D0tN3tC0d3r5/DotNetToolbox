// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System;

public static class Ensure {
    #region Null

    [return: NotNull]
    public static TArgument IsNotNull<TArgument>([NotNull] TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null) {
        ArgumentNullException.ThrowIfNull(argument, paramName);
        return argument;
    }

    [return: NotNull]
    public static TArgument IsDefaultIfNull<TArgument>([NotNull] TArgument? argument, TArgument defaultValue, [CallerArgumentExpression(nameof(defaultValue))] string? paramName = null) {
        argument ??= IsNotNull(defaultValue, paramName);
        return argument;
    }

    #endregion

    #region Type

    [return: NotNull]
    public static TArgument IsOfType<TArgument>([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => IsNotNull(argument, paramName) is not TArgument result
            ? throw new ArgumentException(string.Format(null, ValueMustBeOfType, typeof(TArgument).Name, argument!.GetType().Name), paramName)
            : result;

    public static TArgument? IsOfTypeOrDefault<TArgument>(object? argument, TArgument? defaultValue = default, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => argument switch {
            null => defaultValue,
            TArgument result => result,
            _ => throw new ArgumentException(string.Format(null, ValueMustBeOfType, typeof(TArgument).Name, argument!.GetType().Name), paramName),
        };

    #endregion

    #region Collection

    [return: NotNullIfNotNull(nameof(argument))]
    public static TArgument? IsNotEmpty<TArgument>(TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable
        => argument switch {
            ICollection { Count: 0 } => throw new ArgumentException(CollectionCannotBeEmpty, paramName),
            not null when !argument.Cast<object>().Any() => throw new ArgumentException(CollectionCannotBeEmpty, paramName),
            _ => argument,
        };

    [return: NotNull]
    public static TArgument IsNotNullOrEmpty<TArgument>([NotNull] TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEnumerable
        => IsNotEmpty(IsNotNull(argument, paramName));

    #endregion

    #region String

    public static string IsNotNullOrEmpty([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null) {
        ArgumentException.ThrowIfNullOrEmpty(argument, paramName);
        return argument;
    }

    public static string IsNotNullOrWhiteSpace([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null) {
        ArgumentException.ThrowIfNullOrWhiteSpace(argument, paramName);
        return argument;
    }

    public static string HasDefaultWhenNullOrEmpty([NotNull] string? argument, string defaultValue, [CallerArgumentExpression(nameof(defaultValue))] string? paramName = null) {
        argument = string.IsNullOrEmpty(argument) ? IsNotNullOrWhiteSpace(defaultValue, paramName) : argument;
        return argument;
    }

    public static string HasDefaultWhenNullOrWhiteSpace([NotNull] string? argument, string defaultValue, [CallerArgumentExpression(nameof(defaultValue))] string? paramName = null) {
        argument = string.IsNullOrWhiteSpace(argument) ? IsNotNullOrWhiteSpace(defaultValue, paramName) : argument;
        return argument;
    }

    #endregion

    #region Range

    public static TArgument? IsEqual<TArgument>(TArgument? argument, TArgument? requiredValue, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEquatable<TArgument?> {
        ArgumentOutOfRangeException.ThrowIfNotEqual(argument, requiredValue, paramName);
        return argument;
    }
    public static TArgument? IsNotEqual<TArgument>(TArgument? argument, TArgument? forbiddenValue, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IEquatable<TArgument?> {
        ArgumentOutOfRangeException.ThrowIfEqual(argument, forbiddenValue, paramName);
        return argument;
    }
    public static TArgument IsGreaterThan<TArgument>(TArgument argument, TArgument maximum, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IComparable<TArgument> {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(argument, maximum, paramName);
        return argument;
    }
    public static TArgument IsLessThan<TArgument>(TArgument argument, TArgument minimum, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IComparable<TArgument> {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(argument, minimum, paramName);
        return argument;
    }
    public static TArgument IsNotGreaterThan<TArgument>(TArgument argument, TArgument maximum, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IComparable<TArgument> {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(argument, maximum, paramName);
        return argument;
    }
    public static TArgument IsNotLessThan<TArgument>(TArgument argument, TArgument minimum, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IComparable<TArgument> {
        ArgumentOutOfRangeException.ThrowIfLessThan(argument, minimum, paramName);
        return argument;
    }
    public static TArgument IsZero<TArgument>(TArgument argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : INumberBase<TArgument> {
        ArgumentOutOfRangeException.ThrowIfNotEqual(argument, TArgument.Zero, paramName);
        return argument;
    }
    public static TArgument IsNotZero<TArgument>(TArgument argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : INumberBase<TArgument> {
        ArgumentOutOfRangeException.ThrowIfZero(argument, paramName);
        return argument;
    }
    // Does not include Zero
    public static TArgument IsPositive<TArgument>(TArgument argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : INumberBase<TArgument>, IComparable<TArgument> {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(argument, paramName);
        return argument;
    }
    // Does not include Zero
    public static TArgument IsNegative<TArgument>(TArgument argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : INumberBase<TArgument>, IComparable<TArgument> {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(argument, TArgument.Zero, paramName);
        return argument;
    }
    public static TArgument IsNotNegative<TArgument>(TArgument argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : INumberBase<TArgument>, IComparable<TArgument> {
        ArgumentOutOfRangeException.ThrowIfNegative(argument, paramName);
        return argument;
    }
    public static TArgument IsNotPositive<TArgument>(TArgument argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : INumberBase<TArgument>, IComparable<TArgument> {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(argument, TArgument.Zero, paramName);
        return argument;
    }

    #endregion

    #region IsValid

    [return: NotNull]
    public static TArgument IsValid<TArgument>([NotNull] TArgument? argument, IDictionary<string, object?>? context = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
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

    [return: NotNull]
    public static TArgument IsValid<TArgument>([NotNull] TArgument? argument, Func<TArgument, Result> validate, [CallerArgumentExpression(nameof(argument))] string? paramName = null) {
        var result = validate(IsNotNull(argument, paramName));
        return result.IsSuccess
            ? argument
            : throw new ValidationException(ValueIsNotValid, paramName!);
    }

    [return: NotNullIfNotNull(nameof(defaultValue))]
    public static TArgument? GetDefaultIfInvalid<TArgument>(TArgument? argument, Func<TArgument?, Result> validate, TArgument? defaultValue = default)
        => validate(argument).IsSuccess && argument is not null
               ? argument
               : defaultValue;

    [return: NotNull]
    public static TArgument IsValid<TArgument>([NotNull] TArgument? argument, Func<TArgument?, bool> isValid, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => isValid(IsNotNull(argument, paramName))
               ? argument
               : throw new ValidationException(ValueIsNotValid, paramName!);

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
                   : throw new ValidationException(ValueIsNotValid, paramName!);
    }

    public static async Task<TArgument?> GetDefaultIfIsNotValidAsync<TArgument>(TArgument? argument, Func<TArgument?, Task<Result>> validate, TArgument? defaultValue = default)
        => (await validate(argument)).IsSuccess && argument is not null
               ? argument
               : defaultValue;

    public static async Task<TArgument?> IsValidAsync<TArgument>(TArgument? argument, Func<TArgument?, Task<bool>> isValid, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => await isValid(IsNotNull(argument, paramName))
               ? argument
               : throw new ValidationException(ValueIsNotValid, paramName!);

    public static async Task<TArgument?> GetDefaultIfIsNotValidAsync<TArgument>(TArgument? argument, Func<TArgument?, Task<bool>> isValid, TArgument? defaultValue = default)
        => await isValid(argument) && argument is not null
               ? argument
               : defaultValue;

    #endregion
}
