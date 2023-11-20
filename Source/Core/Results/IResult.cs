namespace System.Results;

public interface IResult {
    bool IsSuccess { get; }
    ISet<ValidationError> Errors { get; }
}
