namespace DotNetToolbox.Results;

public interface IResult {
    ValidationErrors Errors { get; }
    Exception? Exception { get; }
    bool HasErrors { get; }
    [MemberNotNullWhen(true, nameof(Exception))]
    bool HasException { get; }
    void EnsureIsSuccess(string? message = null, string? source = null);
}

public interface IResult<out TType>
    : IResult {
    TType Type { get; }
}

public interface IResult<out TType, out TValue>
    : IResult<TType> {
    TValue? Value { get; }
}
