namespace System.Results;

public interface IResult {
    IReadOnlyList<ValidationError> ValidationErrors { get; }

    void EnsureIsValid(string? message = null);
}

public interface IResult<out TType> : IResult
    where TType : Enum {
    TType Type { get; }
}
