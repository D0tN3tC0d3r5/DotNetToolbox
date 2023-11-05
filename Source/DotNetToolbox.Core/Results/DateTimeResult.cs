namespace System.Results;

public class DateTimeResult : Result<DateTime> {
    internal DateTimeResult(DateTime value, IEnumerable<ValidationError>? errors = null)
        : base(value, errors) {
    }

    public static DateTimeResult operator +(DateTimeResult left, Result right)
        => (DateTimeResult)((Result)left + right);
}
