namespace DotNetToolbox.AI.Graph;

public abstract class Node(int id = 0, INode? caller = null)
    : Node<object>(id, caller);

public abstract class Node<TData>(int id = 0, INode? caller = null)
    : Node<TData, TData>(id, caller)
    , INode<TData>;

-public abstract class Node<TState, TInput, TOutput>(TState state, int id = 0, INode? caller = null)
    : INode<TInput, TOutput> {
    public int Id => id;
    public INode? Caller => caller;
    public virtual INodeResult<TOutput> Execute(TInput? input) {
        var result = Process(input);
        var next = SelectNext(result);
        return new NodeResult<TOutput>(next, result);
    }

    protected abstract TOutput? Process(TInput? input);
    protected abstract INode? SelectNext(TOutput? output);
}
