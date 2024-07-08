namespace DotNetToolbox.AI.Graph;

public class IfThenElseNode<TData>(int id = 0)
    : Node<TData>(id) {
    protected sealed override TData? Process(TData? input) => input;

    protected sealed override INode? Select(TData? input) => Predicate(input, TrueNode, FalseNode);

    protected abstract INode? Predicate(TData? input, INode trueNode, INode falseNode);
}

public class NullNode<TData>(INode? caller = null)
    : Node<TData>(Guid.Empty.ToString(), caller, i => i, s => caller!);
