namespace System.Results;

public interface IResult {
    bool IsSuccess { get; }
    IList<ValidationError> Errors { get; }
}
