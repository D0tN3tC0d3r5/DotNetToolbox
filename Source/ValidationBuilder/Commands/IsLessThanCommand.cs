namespace DotNetToolbox.ValidationBuilder.Commands;

public sealed class IsLessThanCommand<TValue>
    : ValidationCommand
    where TValue : IComparable<TValue> {
    public IsLessThanCommand(TValue threshold, string source)
        : base(source) {
        ValidateAs = v => ((TValue)v).CompareTo(threshold) < 0;
        ValidationErrorMessage = MustBeLessThan;
        GetErrorMessageArguments = v => new[] { threshold, v! };
    }
}
