using DotNetToolbox.Validation;

namespace DotNetToolbox.Results;

public interface IResult
{
    IReadOnlyList<IValidationError> ValidationErrors { get; }
}

public interface IResult<out TType> : IResult
    where TType : Enum
{
    TType Type { get; }
}
