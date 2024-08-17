namespace DotNetToolbox.Graph.Factories;

public interface INodeFactory {
    TNode Create<TNode>(string id,
                        string? label = null)
        where TNode : Node<TNode>;
    TNode Create<TNode>(string? label = null)
        where TNode : Node<TNode>;

    INode CreateAction(string id,
                       Action<Context> action,
                       string? label = null);
    INode CreateAction(Action<Context> action,
                       string? label = null);

    INode CreateIf(string id,
                   Func<Context, bool> predicate,
                   INode truePath,
                   INode? falsePath = null,
                   string? label = null);
    INode CreateIf(Func<Context, bool> predicate,
                   INode truePath,
                   INode? falsePath = null,
                   string? label = null);
    INode CreateCase(string id,
                     Func<Context, string> selectPath,
                     Dictionary<string, INode?> choices,
                     INode? otherwise = null,
                     string? label = null);
    INode CreateCase(Func<Context, string> selectPath,
                     Dictionary<string, INode?> choices,
                     INode? otherwise = null,
                     string? label = null);

    INode CreateJump(string id,
                     string targetTag,
                     string? label = null);
    INode CreateJump(string targetTag,
                     string? label = null);

    INode CreateExit(int exitCode = 0,
                     string? label = null);
    INode CreateExit(string id,
                     int exitCode = 0,
                     string? label = null);
}
