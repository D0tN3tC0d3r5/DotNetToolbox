namespace DotNetToolbox.AI.Graph;

public record NodeResult(INode? Next, object? Value = default)
    : NodeResult<object>(Next, Value);

public record NodeResult<TValue>(INode? Next, TValue? Value = default)
    : INodeResult<TValue>;
