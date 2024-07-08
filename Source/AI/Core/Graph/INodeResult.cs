namespace DotNetToolbox.AI.Graph;

public interface INodeResult {
    INode? Next { get; }
    object? Value { get; }
}

public interface INodeResult<out TValue>
    : INodeResult {
    object? INodeResult.Value => Value;
    new TValue? Value { get; }
}
