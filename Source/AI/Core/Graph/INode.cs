namespace DotNetToolbox.AI.Graph;

public interface INode<out TState> {
    int Id { get; }
    TState State { get; }
    INode<TState>? Caller { get; }
    INodeResult Execute(object? input);
}

public interface INode<out TState, TData>
    : INode<TState, TData, TData> {
}

public interface INode<out TState, in TInput, out TOutput>
    : INode<TState> {
    INodeResult INode<TState>.Execute(object? input) => Execute(input);
    INodeResult<TOutput> Execute(TInput? input);
}
