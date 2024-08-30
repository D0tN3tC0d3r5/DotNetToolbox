namespace System;

public static partial class Ensure {
    public static Task<TArgument?> IsValidAsync<TArgument>(TArgument? argument, IContext? context = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where TArgument : IValidatableAsync
        => IsValidAsync(IsNotNull(argument, paramName), arg => arg.Validate(context), paramName);

    public static async Task<TArgument?> DefaultIfNotValidAsync<TArgument>(TArgument? argument, IContext? context = null, TArgument? defaultValue = default)
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

    public static async Task<TArgument?> DefaultIfNotValidAsync<TArgument>(TArgument? argument, Func<TArgument?, Task<Result>> validate, TArgument? defaultValue = default)
        => (await validate(argument)).IsSuccess && argument is not null
               ? argument
               : defaultValue;

    public static async Task<TArgument?> IsValidAsync<TArgument>(TArgument? argument, Func<TArgument?, Task<bool>> isValid, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => await isValid(IsNotNull(argument, paramName))
               ? argument
               : throw new ValidationException(ValueIsNotValid, paramName!);

    public static async Task<TArgument?> DefaultIfNotValidAsync<TArgument>(TArgument? argument, Func<TArgument?, Task<bool>> isValid, TArgument? defaultValue = default)
        => await isValid(argument) && argument is not null
               ? argument
               : defaultValue;
}
