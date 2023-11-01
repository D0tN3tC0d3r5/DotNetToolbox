namespace DotNetToolbox.ValidationBuilder.Commands;

public abstract class ValidationCommand(string source) : IValidationCommand {
    protected string Source { get; } = source;

    public virtual Result Validate(object? subject)
        => subject is null || ValidateAs(subject)
                                                           ? Result.Success()
                                                           : Result.Invalid(Source, ValidationErrorMessage, GetErrorMessageArguments(subject));

    public virtual Result Negate(object? subject) => subject is null || !ValidateAs(subject)
                                                         ? Result.Success()
                                                         : Result.Invalid(Source, InvertMessage(ValidationErrorMessage), GetErrorMessageArguments(subject));

    protected Func<object, bool> ValidateAs { get; init; } = _ => true;
    protected string ValidationErrorMessage { get; init; } = MustBeValid;
    protected Func<object?, object[]> GetErrorMessageArguments { get; init; } = _ => Array.Empty<object>();
}
