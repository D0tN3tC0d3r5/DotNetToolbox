namespace DotNetToolbox.Results;

public interface IResult {
    ISet<ValidationError> Errors { get; }
    Exception? Exception { get; }
}

public interface IResult<out TValue> : IResult {
    TValue? Value { get; }
}
