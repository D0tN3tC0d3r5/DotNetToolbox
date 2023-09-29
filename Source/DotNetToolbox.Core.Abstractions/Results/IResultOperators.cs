namespace System.Results;

public interface IResultOperators<TSelf> :
    IAddValidationResult<TSelf>,
    IAddErrors<TSelf>,
    IAddError<TSelf>
    where TSelf : IResultOperators<TSelf>
{
}