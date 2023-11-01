namespace DotNetToolbox.ValidationBuilder.Commands;

public sealed class LengthIsAtMostCommand
    : ValidationCommand {
    public LengthIsAtMostCommand(int length, string source)
        : base(source) {
        ValidateAs = s => ((string)s).Length <= length;
        ValidationErrorMessage = MustHaveAMaximumLengthOf;
        GetErrorMessageArguments = s => new object?[] { length, ((string)s!).Length, };
    }
}
