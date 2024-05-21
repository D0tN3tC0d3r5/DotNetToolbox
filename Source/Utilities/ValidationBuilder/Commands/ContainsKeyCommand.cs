namespace DotNetToolbox.ValidationBuilder.Commands;

public sealed class ContainsKeyCommand<TKey, TValue>
    : ValidationCommand
    where TKey : notnull {
    public ContainsKeyCommand(TKey key, string source)
        : base(source) {
        ValidateAs = d => ((IDictionary<TKey, TValue?>)d).ContainsKey(key);
        ValidationErrorMessage = MustContainKey;
        GetErrorMessageArguments = _ => [key];
    }
}
