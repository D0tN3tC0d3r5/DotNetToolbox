using System.Validation;

namespace System.Results;

public interface IAddErrors<TSelf> :
    IAdditionOperators<TSelf, IEnumerable<IValidationError>, TSelf>
    where TSelf : IAddErrors<TSelf> {
}