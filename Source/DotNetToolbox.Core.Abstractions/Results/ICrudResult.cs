namespace System.Results;

public interface ICrudResult : IResult<CrudResultType>
{
    bool IsFailure { get; }
    bool IsSuccess { get; }
    bool IsNotFound { get; }
    bool IsConflict { get; }
}

public interface ICrudResult<out TValue> : ICrudResult
{
    TValue? Value { get; }

    ICrudResult<TNewValue> Map<TNewValue>(Func<TValue, TNewValue> map);
}
