namespace System.Results;

public class BoolResult : Result<bool> {
    internal BoolResult(bool value, IEnumerable<ValidationError>? errors = null)
        : base(value, errors) {
    }

    public bool IsTrue => Value;
    public bool IsFalse => !Value;

    public static BoolResult operator +(BoolResult left, Result right)
        => (BoolResult)((Result)left + right);
}
