namespace DotNetToolbox.ValidationBuilder.Commands;

public sealed class IsAfterCommand
    : ValidationCommand {
    public IsAfterCommand(DateTime @event, string source)
        : base(source) {
        ValidateAs = dt => (DateTime)dt > @event;
        ValidationErrorMessage = MustBeAfter;
        GetErrorMessageArguments = dt => new object?[] { @event, (DateTime)dt!, };
    }
}
