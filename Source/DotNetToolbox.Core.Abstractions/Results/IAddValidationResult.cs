namespace System.Results;

public interface IAddValidationResult<TSelf> :
    IAdditionOperators<TSelf, IValidationResult, TSelf>
    where TSelf : IAddValidationResult<TSelf> {
}