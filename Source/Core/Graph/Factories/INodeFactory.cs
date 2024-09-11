namespace DotNetToolbox.Graph.Factories;

public interface INodeFactory {
    TNode Create<TNode>(string? tag = null, params object[] args)
        where TNode : Node<TNode>;

    IActionNode CreateAction(Func<Map, CancellationToken, Task> action);
    IActionNode CreateAction(string tag, Func<Map, CancellationToken, Task> action);
    IActionNode CreateAction(Action<Map> action);
    IActionNode CreateAction(string tag, Action<Map> action);

    IIfNode CreateIf(Func<Map, CancellationToken, Task<bool>> predicate);
    IIfNode CreateIf(string tag, Func<Map, CancellationToken, Task<bool>> predicate);
    IIfNode CreateIf(string tag,
                     Func<Map, CancellationToken, Task<bool>> predicate,
                     INode truePath,
                     INode? falsePath = null);
    IIfNode CreateIf(Func<Map, CancellationToken, Task<bool>> predicate,
                   INode truePath,
                   INode? falsePath = null);
    IIfNode CreateIf(Func<Map, bool> predicate);
    IIfNode CreateIf(string tag, Func<Map, bool> predicate);
    IIfNode CreateIf(string tag,
                     Func<Map, bool> predicate,
                     INode truePath,
                     INode? falsePath = null);
    IIfNode CreateIf(Func<Map, bool> predicate,
                   INode truePath,
                   INode? falsePath = null);

    ICaseNode CreateCase(Func<Map, CancellationToken, Task<string>> selectPath);
    ICaseNode CreateCase(string tag, Func<Map, CancellationToken, Task<string>> selectPath);
    ICaseNode CreateCase(string tag,
                         Func<Map, CancellationToken, Task<string>> selectPath,
                         Dictionary<string, INode?> choices,
                         INode? otherwise = null);
    ICaseNode CreateCase(Func<Map, CancellationToken, Task<string>> selectPath,
                         Dictionary<string, INode?> choices,
                         INode? otherwise = null);
    ICaseNode CreateCase(Func<Map, string> selectPath);
    ICaseNode CreateCase(string tag, Func<Map, string> selectPath);
    ICaseNode CreateCase(string tag,
                         Func<Map, string> selectPath,
                         Dictionary<string, INode?> choices,
                         INode? otherwise = null);
    ICaseNode CreateCase(Func<Map, string> selectPath,
                         Dictionary<string, INode?> choices,
                         INode? otherwise = null);

    IJumpNode CreateJump(string targetTag);

    IExitNode CreateExit(int exitCode = 0);
    IExitNode CreateExit(string tag, int exitCode = 0);
}
