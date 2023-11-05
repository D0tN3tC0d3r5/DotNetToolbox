namespace System.Results;

public class TimeSpanResult : Result<TimeSpan> {
    internal TimeSpanResult(TimeSpan value, IEnumerable<ValidationError>? errors = null)
        : base(value, errors) {
    }

    public static TimeSpanResult operator +(TimeSpanResult left, Result right)
        => (TimeSpanResult)((Result)left + right);
}
