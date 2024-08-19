//namespace DotNetToolbox.Graph.Factories;

//public interface INodeFactory {
//    TNode Create<TNode>(string? id = null)
//        where TNode : Node<TNode>;

//    IActionNode CreateAction(Action<Context> action);
//    IActionNode CreateAction(string id, Action<Context> action);

//    IIfNode CreateIf(Func<Context, bool> predicate);
//    IIfNode CreateIf(string id, Func<Context, bool> predicate);
//    IIfNode CreateIf(string id,
//                     Func<Context, bool> predicate,
//                     INode truePath,
//                     INode? falsePath = null);
//    IIfNode CreateIf(Func<Context, bool> predicate,
//                   INode truePath,
//                   INode? falsePath = null);

//    ICaseNode CreateCase(Func<Context, string> selectPath);
//    ICaseNode CreateCase(string id, Func<Context, string> selectPath);
//    ICaseNode CreateCase(string id,
//                         Func<Context, string> selectPath,
//                         Dictionary<string, INode?> choices,
//                         INode? otherwise = null);
//    ICaseNode CreateCase(Func<Context, string> selectPath,
//                         Dictionary<string, INode?> choices,
//                         INode? otherwise = null);

//    IJumpNode CreateJump(string targetTag);
//    IJumpNode CreateJump(string id, string targetTag);

//    IExitNode CreateExit(int exitCode = 0);
//    IExitNode CreateExit(string id, int exitCode = 0);
//}
