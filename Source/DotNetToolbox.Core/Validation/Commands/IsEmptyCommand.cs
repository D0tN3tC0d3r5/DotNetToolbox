namespace System.Validation.Commands;

public sealed class IsEmptyCommand
    : ValidationCommand {
    public IsEmptyCommand(string source)
        : base(source) {
        ValidateAs = s => ((string)s).Length == 0;
        ValidationErrorMessage = MustBeEmpty;
    }
}

public sealed class IsEmptyCommand<TItem>
    : ValidationCommand {
    public IsEmptyCommand(string source)
        : base(source) {
        ValidateAs = c => ((ICollection<TItem?>)c).Count == 0;
        ValidationErrorMessage = MustBeEmpty;
    }
}