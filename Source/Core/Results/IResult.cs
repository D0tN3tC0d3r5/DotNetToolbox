namespace DotNetToolbox.Results;

public interface IResult {
    IReadOnlyList<ValidationError> Errors { get; }

    bool HasErrors { get; }
    [MemberNotNullWhen(true, nameof(InnerException))]
    bool HasException { get; }
    Exception? InnerException { get; }

    void EnsureIsSuccess();
}

public interface IResult<out TValue> : IResult {
    TValue? Value { get; }
}
