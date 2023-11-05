namespace System.Results;

public class DoubleResult : Result<double> {
    internal DoubleResult(double value, IEnumerable<ValidationError>? errors = null)
        : base(value, errors) {
    }

    public static DoubleResult operator +(DoubleResult left, Result right)
        => (DoubleResult)((Result)left + right);
}
