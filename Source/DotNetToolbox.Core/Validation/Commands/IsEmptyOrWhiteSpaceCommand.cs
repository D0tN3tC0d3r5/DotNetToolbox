namespace System.Validation.Commands;

public sealed class IsEmptyOrWhiteSpaceCommand
    : ValidationCommand {
    public IsEmptyOrWhiteSpaceCommand(string source)
        : base(source) {
        ValidateAs = s => ((string)s).Trim().Length == 0;
        ValidationErrorMessage = MustBeEmptyOrWhiteSpace;
    }
}