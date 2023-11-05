namespace System.Results;

public interface IResult {
    ISet<ValidationError> Errors { get; }
}
