namespace DotNetToolbox.AI.Graph;

public abstract class ConditionalNode<TData>(string id, INode caller, Func<TData?, INode>? select = null)
    : Node<TData>(id, caller, i => i, select) {

    protected ConditionalNode(INode caller, Func<TData?, INode>? select = null) :
        this(null!, caller, select) {
    }

    protected ConditionalNode(string id, Func<TData?, INode>? select = null) :
        this(id, null!, select) {
    }

    protected ConditionalNode(Func<TData?, INode>? select = null) :
        this(null!, null!, select) {
    }
}
