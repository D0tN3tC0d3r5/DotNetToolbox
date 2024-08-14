namespace DotNetToolbox.Graph.Nodes;

public interface INodeFactory {
    TNode Create<TNode>(uint id, string? tag = null, string? label = null)
        where TNode : Node<TNode>;

    IConditionalNode CreateFork(uint id,
                                Func<Context, bool> predicate,
                                Action<IConditionalNode, ConditionalNodeBuilder> setPaths,
                                string? tag = null,
                                string? label = null);
    IBranchingNode CreateChoice(uint id,
                                Func<Context, string> selectPath,
                                Action<IBranchingNode, BranchingNodeBuilder> setPaths,
                                string? tag = null,
                                string? label = null);

    IActionNode CreateAction(uint id, Action<Context> action, string? tag = null, string? label = null);

    IJumpNode CreateJump(uint id, string targetTag, string? label = null);

    ITerminationNode CreateStop(uint id, int exitCode = 0, string? tag = null, string? label = null);
}
