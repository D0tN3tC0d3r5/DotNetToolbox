namespace DotNetToolbox.Graph.Factories;

public interface INodeFactory {
    TNode Create<TNode>(uint id,
                        string? tag = null,
                        string? label = null)
        where TNode : Node<TNode>;

    INode CreateFork(uint id,
                             Func<Context, bool> predicate,
                             Action<IIfNodeBuilder> setPaths,
                             string? tag = null,
                             string? label = null);
    INode CreateChoice(uint id,
                               Func<Context, string> selectPath,
                               Action<ICaseNodeBuilder> setPaths,
                               string? tag = null,
                               string? label = null);

    INode CreateAction(uint id,
                       Action<Context> action,
                       string? tag = null,
                       string? label = null);

    INode CreateJump(uint id,
                     string targetTag,
                     string? tag = null,
                     string? label = null);

    INode CreateExit(uint id,
                     int exitCode = 0,
                     string? tag = null,
                     string? label = null);
}
