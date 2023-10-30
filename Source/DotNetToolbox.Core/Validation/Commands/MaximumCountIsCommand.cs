namespace System.Validation.Commands;

public sealed class HasAtMostCommand<TItem>
    : ValidationCommand {
    public HasAtMostCommand(int count, string source)
        : base(source) {
        ValidateAs = o => ((ICollection<TItem?>)o).Count <= count;
        ValidationErrorMessage = MustHaveAMaximumCountOf;
        GetErrorMessageArguments = c => new object?[] { count, ((ICollection<TItem?>)c!).Count };
    }
}
