using DotNetToolbox.Validation;

namespace DotNetToolbox.Results;

public interface IAddErrors<TSelf> :
    IAdditionOperators<TSelf, IEnumerable<IValidationError>, TSelf>
    where TSelf : IAddErrors<TSelf>
{
}