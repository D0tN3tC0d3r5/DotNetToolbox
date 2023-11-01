namespace DotNetToolbox.ValidationBuilder.Commands;

public sealed class IsBeforeCommand
    : ValidationCommand {
    public IsBeforeCommand(DateTime @event, string source)
        : base(source) {
        ValidateAs = dt => (DateTime)dt < @event;
        ValidationErrorMessage = MustBeBefore;
        GetErrorMessageArguments = dt => new object?[] { @event, (DateTime)dt!, };
    }
}
