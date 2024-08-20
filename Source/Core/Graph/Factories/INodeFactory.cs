namespace DotNetToolbox.Graph.Factories;

public interface INodeFactory {
    TNode Create<TNode>(string? tag = null)
        where TNode : Node<TNode>;

    IActionNode CreateAction(Func<Context, CancellationToken, Task> action);
    IActionNode CreateAction(string tag, Func<Context, CancellationToken, Task> action);
    IActionNode CreateAction(Action<Context> action);
    IActionNode CreateAction(string tag, Action<Context> action);

    IIfNode CreateIf(Func<Context, CancellationToken, Task<bool>> predicate);
    IIfNode CreateIf(string tag, Func<Context, CancellationToken, Task<bool>> predicate);
    IIfNode CreateIf(string tag,
                     Func<Context, CancellationToken, Task<bool>> predicate,
                     INode truePath,
                     INode? falsePath = null);
    IIfNode CreateIf(Func<Context, CancellationToken, Task<bool>> predicate,
                   INode truePath,
                   INode? falsePath = null);
    IIfNode CreateIf(Func<Context, bool> predicate);
    IIfNode CreateIf(string tag, Func<Context, bool> predicate);
    IIfNode CreateIf(string tag,
                     Func<Context, bool> predicate,
                     INode truePath,
                     INode? falsePath = null);
    IIfNode CreateIf(Func<Context, bool> predicate,
                   INode truePath,
                   INode? falsePath = null);

    ICaseNode CreateCase(Func<Context, CancellationToken, Task<string>> selectPath);
    ICaseNode CreateCase(string tag, Func<Context, CancellationToken, Task<string>> selectPath);
    ICaseNode CreateCase(string tag,
                         Func<Context, CancellationToken, Task<string>> selectPath,
                         Dictionary<string, INode?> choices,
                         INode? otherwise = null);
    ICaseNode CreateCase(Func<Context, CancellationToken, Task<string>> selectPath,
                         Dictionary<string, INode?> choices,
                         INode? otherwise = null);
    ICaseNode CreateCase(Func<Context, string> selectPath);
    ICaseNode CreateCase(string tag, Func<Context, string> selectPath);
    ICaseNode CreateCase(string tag,
                         Func<Context, string> selectPath,
                         Dictionary<string, INode?> choices,
                         INode? otherwise = null);
    ICaseNode CreateCase(Func<Context, string> selectPath,
                         Dictionary<string, INode?> choices,
                         INode? otherwise = null);

    IJumpNode CreateJump(string targetTag);

    IExitNode CreateExit(int exitCode = 0);
    IExitNode CreateExit(string tag, int exitCode = 0);
}
