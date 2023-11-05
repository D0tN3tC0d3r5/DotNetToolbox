namespace System.Results;

public class DictionaryResult<TKey, TValue> : Result<IReadOnlyDictionary<TKey, TValue>>
    where TKey : notnull {
    internal DictionaryResult(IReadOnlyDictionary<TKey, TValue> values, IEnumerable<ValidationError>? errors = null)
        : base(values, errors) {
    }

    public static DictionaryResult<TKey, TValue> operator +(DictionaryResult<TKey, TValue> left, Result right)
        => (DictionaryResult<TKey, TValue>)((Result)left + right);
}
