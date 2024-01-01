namespace DotNetToolbox.Results;

public interface IResult {
    bool IsSuccess { get; }
    ISet<ValidationError> Errors { get; }
    Exception? Exception { get; }
}
