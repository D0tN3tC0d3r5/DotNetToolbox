namespace DotNetToolbox.ValidationBuilder.Commands;

public sealed class LengthIsAtLeastCommand : ValidationCommand {
    public LengthIsAtLeastCommand(int length, string source)
        : base(source) {
        ValidateAs = s => ((string)s).Length >= length;
        ValidationErrorMessage = MustHaveAMinimumLengthOf;
        GetErrorMessageArguments = s => [length, ((string)s!).Length];
    }
}
