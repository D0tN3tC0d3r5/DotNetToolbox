namespace System.Validation.Commands;

public sealed class ContainsValueCommand<TKey, TValue>
    : ValidationCommand
    where TKey : notnull {
    public ContainsValueCommand(TValue? value, string source)
        : base(source) {
        ValidateAs = d => ((IDictionary<TKey, TValue?>)d).Values.Contains(value);
        ValidationErrorMessage = MustContainValue;
        GetErrorMessageArguments = _ => new object?[] { value };
    }
}