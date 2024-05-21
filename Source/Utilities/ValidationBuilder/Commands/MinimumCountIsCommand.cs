namespace DotNetToolbox.ValidationBuilder.Commands;

public sealed class HasAtLeastCommand<TItem>
    : ValidationCommand {
    public HasAtLeastCommand(int count, string source)
        : base(source) {
        ValidateAs = c => ((ICollection<TItem?>)c).Count >= count;
        ValidationErrorMessage = MustHaveAMinimumCountOf;
        GetErrorMessageArguments = c => [count, ((ICollection<TItem?>)c!).Count];
    }
}
