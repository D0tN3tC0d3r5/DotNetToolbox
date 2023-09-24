namespace DotNetToolbox.Results;

public interface IValidationResult : IResult<ValidationResultType>
{
    bool IsFailure { get; }
    bool IsSuccess { get; }
}
