namespace System.Results;

public interface IAddErrors<TSelf> :
    IAdditionOperators<TSelf, IEnumerable<ValidationError>, TSelf>
    where TSelf : IAddErrors<TSelf> {
}