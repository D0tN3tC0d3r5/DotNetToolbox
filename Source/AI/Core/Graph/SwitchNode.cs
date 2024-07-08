namespace DotNetToolbox.AI.Graph;

public abstract class SwitchNode<TData>(int id = 0)
    : Node<TData>(id) {
    protected sealed override TData? Process(TData? input) => input;
}
