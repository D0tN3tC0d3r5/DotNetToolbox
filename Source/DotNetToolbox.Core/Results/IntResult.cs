namespace System.Results;

public class IntResult : Result<int> {
    internal IntResult(int value, IEnumerable<ValidationError>? errors = null)
        : base(value, errors) {
    }

    public static IntResult operator +(IntResult left, Result right)
        => (IntResult)((Result)left + right);
}
