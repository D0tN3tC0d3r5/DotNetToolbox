namespace DotNetToolbox.ValidationBuilder.Commands;

public sealed class LengthIsCommand
    : ValidationCommand {
    public LengthIsCommand(int length, string source)
        : base(source) {
        ValidateAs = s => ((string)s).Length == length;
        ValidationErrorMessage = MustHaveALengthOf;
        GetErrorMessageArguments = s => new object[] { length, ((string)s!).Length, };
    }
}
