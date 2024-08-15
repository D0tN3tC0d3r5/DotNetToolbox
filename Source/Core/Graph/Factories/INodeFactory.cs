namespace DotNetToolbox.Graph.Factories;

public interface INodeFactory {
    TNode Create<TNode>(uint id, string? tag = null, string? label = null)
        where TNode : Node<TNode>;

    IIfNode CreateFork(uint id,
                                Func<Context, bool> predicate,
                                Action<IfNodeBuilder> setPaths,
                                string? tag = null,
                                string? label = null);
    ICaseNode CreateChoice(uint id,
                                Func<Context, string> selectPath,
                                Action<CaseNodeBuilder> setPaths,
                                string? tag = null,
                                string? label = null);

    IActionNode CreateAction(uint id, Action<Context> action, string? tag = null, string? label = null);

    IJumpNode CreateJump(uint id, string targetTag, string? label = null);

    IEndNode CreateExit(uint id, int exitCode = 0, string? tag = null, string? label = null);
}
