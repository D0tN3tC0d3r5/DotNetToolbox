namespace DotNetToolbox.ValidationBuilder.Commands;

public sealed class IsNullCommand : ValidationCommand {
    public IsNullCommand(string source)
        : base(source) {
        ValidationErrorMessage = MustBeNull;
    }

    public override Result Validate(object? subject)
        => subject is null
        ? Result.Success()
        : Result.InvalidData(Source, string.Format(ValidationErrorMessage, GetErrorMessageArguments(subject)));

    public override Result Negate(object? subject)
        => subject is not null
        ? Result.Success()
        : Result.InvalidData(Source, string.Format(InvertMessage(ValidationErrorMessage), GetErrorMessageArguments(subject)));
}
