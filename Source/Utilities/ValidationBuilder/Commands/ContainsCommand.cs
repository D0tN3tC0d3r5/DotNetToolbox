namespace DotNetToolbox.ValidationBuilder.Commands;

public sealed class ContainsCommand
    : ValidationCommand {
    public ContainsCommand(string subString, string source)
        : base(source) {
        ValidateAs = s => ((string)s).Contains(subString);
        ValidationErrorMessage = MustContain;
        GetErrorMessageArguments = _ => [subString];
    }
}

public sealed class ContainsCommand<TItem>
    : ValidationCommand {
    public ContainsCommand(TItem? item, string source)
        : base(source) {
        ValidateAs = c => ((ICollection<TItem?>)c).Contains(item);
        ValidationErrorMessage = MustContain;
        GetErrorMessageArguments = _ => [item!];
    }
}
