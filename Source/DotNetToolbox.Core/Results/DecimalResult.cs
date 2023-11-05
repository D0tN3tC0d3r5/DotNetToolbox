namespace System.Results;

public class DecimalResult : Result<decimal> {
    internal DecimalResult(decimal value, IEnumerable<ValidationError>? errors = null)
        : base(value, errors) {
    }

    public static DecimalResult operator +(DecimalResult left, Result right)
        => (DecimalResult)((Result)left + right);
}
