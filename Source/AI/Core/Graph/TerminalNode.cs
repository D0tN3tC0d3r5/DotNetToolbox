namespace DotNetToolbox.AI.Graph;

public class TerminalNode<TFinalState>(string id, TFinalState? input = default)
    : Node<TFinalState, TFinalState>(id, input) {
    public TerminalNode(TFinalState? input = default)
        : this(Guid.NewGuid().ToString(), input) {
    }

    public sealed override NodeResult<TFinalState> Execute()
        => new End<TFinalState>(State);
}
