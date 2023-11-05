namespace System.Results;

public class LongResult : Result<long> {
    internal LongResult(long value, IEnumerable<ValidationError>? errors = null)
        : base(value, errors) {
    }

    public static LongResult operator +(LongResult left, Result right)
        => (LongResult)((Result)left + right);
}
