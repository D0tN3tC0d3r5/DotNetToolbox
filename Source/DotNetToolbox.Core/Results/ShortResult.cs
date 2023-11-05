namespace System.Results;

public class ShortResult : Result<short> {
    internal ShortResult(short value, IEnumerable<ValidationError>? errors = null)
        : base(value, errors) {
    }

    public static ShortResult operator +(ShortResult left, Result right)
        => (ShortResult)((Result)left + right);
}
