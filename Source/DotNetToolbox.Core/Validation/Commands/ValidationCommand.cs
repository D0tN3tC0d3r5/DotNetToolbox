namespace System.Validation.Commands;

public abstract class ValidationCommand : IValidationCommand {
    protected ValidationCommand(string source) {
        Source = source;
    }

    protected string Source { get; }

    public virtual Result Validate(object? subject) {
        if (subject is null || ValidateAs(subject)) return Result.Success();
        return Result.Invalid(ValidationErrorMessage, Source, GetErrorMessageArguments(subject));
    }

    public virtual Result Negate(object? subject) {
        if (subject is null || !ValidateAs(subject)) return Result.Success();
        return Result.Invalid(InvertMessage(ValidationErrorMessage), Source, GetErrorMessageArguments(subject));
    }

    protected Func<object, bool> ValidateAs { get; init; } = _ => true;
    protected string ValidationErrorMessage { get; init; } = MustBeValid;
    protected Func<object?, object?[]> GetErrorMessageArguments { get; init; } = _ => Array.Empty<object?>();
}
