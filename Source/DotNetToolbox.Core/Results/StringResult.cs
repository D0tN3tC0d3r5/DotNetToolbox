namespace System.Results;

public class StringResult : Result<string> {
    internal StringResult(string value, IEnumerable<ValidationError>? errors = null)
        : base(value, errors) {
    }

    public static StringResult operator +(StringResult left, Result right)
        => (StringResult)((Result)left + right);
}
