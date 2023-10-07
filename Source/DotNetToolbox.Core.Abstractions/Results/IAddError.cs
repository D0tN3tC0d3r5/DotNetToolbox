using System.Validation;

namespace System.Results;

public interface IAddError<TSelf> :
    IAdditionOperators<TSelf, ValidationError, TSelf>
    where TSelf : IAddError<TSelf> {
}