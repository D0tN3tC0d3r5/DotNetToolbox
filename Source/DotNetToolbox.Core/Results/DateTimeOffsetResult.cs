namespace System.Results;

public class DateTimeOffsetResult : Result<DateTimeOffset> {
    internal DateTimeOffsetResult(DateTimeOffset value, IEnumerable<ValidationError>? errors = null)
        : base(value, errors) {
    }

    public static DateTimeOffsetResult operator +(DateTimeOffsetResult left, Result right)
        => (DateTimeOffsetResult)((Result)left + right);
}
