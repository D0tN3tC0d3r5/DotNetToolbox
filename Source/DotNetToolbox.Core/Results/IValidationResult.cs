namespace System.Results;

public interface IValidationResult : IResult<ValidationResultType> {
    bool IsSuccess { get; }
    bool IsFailure { get; }
}
