namespace DotNetToolbox.ValidationBuilder.Commands;

public sealed class HasCommand<TItem>
    : ValidationCommand {
    public HasCommand(int count, string source)
        : base(source) {
        ValidateAs = c => ((ICollection<TItem?>)c).Count == count;
        ValidationErrorMessage = MustHaveACountOf;
        GetErrorMessageArguments = c => [count, ((ICollection<TItem?>)c!).Count];
    }
}
