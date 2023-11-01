namespace DotNetToolbox.ValidationBuilder.Commands;

public sealed class IsGreaterThanCommand<TValue>
    : ValidationCommand
    where TValue : IComparable<TValue> {
    public IsGreaterThanCommand(TValue threshold, string source)
        : base(source) {
        ValidateAs = v => ((TValue)v).CompareTo(threshold) > 0;
        ValidationErrorMessage = MustBeGraterThan;
        GetErrorMessageArguments = v => new[] { threshold, v, };
    }
}