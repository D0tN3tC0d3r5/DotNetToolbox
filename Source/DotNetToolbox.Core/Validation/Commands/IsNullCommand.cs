namespace System.Validation.Commands;

public sealed class IsNullCommand : ValidationCommand {
    public IsNullCommand(string source)
        : base(source) {
        ValidationErrorMessage = MustBeNull;
    }

    public override Result Validate(object? subject) {
        if (subject is null) return Result.Success();
        return Result.Invalid(ValidationErrorMessage, Source, GetErrorMessageArguments(subject));
    }

    public override Result Negate(object? subject) {
        if (subject is not null) return Result.Success();
        return Result.Invalid(InvertMessage(ValidationErrorMessage), Source, GetErrorMessageArguments(subject));
    }
}
