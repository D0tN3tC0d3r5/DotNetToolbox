using DotNetToolbox.Validation;

namespace DotNetToolbox.Results;

public interface IAddError<TSelf> :
    IAdditionOperators<TSelf, IValidationError, TSelf>
    where TSelf : IAddError<TSelf>
{
}