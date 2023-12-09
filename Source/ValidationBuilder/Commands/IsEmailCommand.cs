namespace DotNetToolbox.ValidationBuilder.Commands;

public sealed partial class IsEmailCommand
    : ValidationCommand {
    public IsEmailCommand(string source)
        : base(source) {
        ValidateAs = s => EmailChecker().IsMatch((string)s);
        ValidationErrorMessage = MustBeAValidEmail;
    }

    [GeneratedRegex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$", RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex EmailChecker();
}