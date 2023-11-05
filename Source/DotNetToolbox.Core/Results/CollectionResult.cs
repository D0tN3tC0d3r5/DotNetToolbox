namespace System.Results;

public class CollectionResult<TItem> : Result<IReadOnlyCollection<TItem>> {
    internal CollectionResult(IReadOnlyCollection<TItem> values, IEnumerable<ValidationError>? errors = null)
        : base(values, errors) {
    }

    public static CollectionResult<TItem> operator +(CollectionResult<TItem> left, Result right)
        => (CollectionResult<TItem>)((Result)left + right);
}
