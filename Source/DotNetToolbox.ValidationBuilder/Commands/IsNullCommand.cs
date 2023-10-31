namespace DotNetToolbox.ValidationBuilder.Commands;

public sealed class IsNullCommand : ValidationCommand {
    public IsNullCommand(string source)
        : base(source) {
        ValidationErrorMessage = MustBeNull;
    }

    public override Result Validate(object? subject) {
        if (subject is null) return Result.Success();
        return Result.Invalid(Source, ValidationErrorMessage, GetErrorMessageArguments(subject));
    }

    public override Result Negate(object? subject) {
        if (subject is not null) return Result.Success();
        return Result.Invalid(Source, InvertMessage(ValidationErrorMessage), GetErrorMessageArguments(subject));
    }
}
