namespace DotNetToolbox.ValidationBuilder.Commands;

public sealed class HasAtMostCommand<TItem>
    : ValidationCommand {
    public HasAtMostCommand(int count, string source)
        : base(source) {
        ValidateAs = o => ((ICollection<TItem?>)o).Count <= count;
        ValidationErrorMessage = MustHaveAMaximumCountOf;
        GetErrorMessageArguments = c => [count, ((ICollection<TItem?>)c!).Count];
    }
}
