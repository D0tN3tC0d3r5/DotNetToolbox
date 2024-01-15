namespace DotNetToolbox.Results;

public interface IResult {
    IReadOnlyList<ValidationError> Errors { get; }

    bool HasErrors { get; }
    [MemberNotNullWhen(true, nameof(Exception))]
    bool HasException { get; }
    Exception? Exception { get; }

    void EnsureIsSuccess(string? message = null, string? source = null);
}

public interface IResult<out TValue> : IResult {
    TValue? Value { get; }
}
