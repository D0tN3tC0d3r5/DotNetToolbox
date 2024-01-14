namespace DotNetToolbox.Results;

public interface IResult {
    ICollection<ValidationError> Errors { get; }
}

public interface IResult<out TValue> : IResult {
    TValue? Value { get; }
}
