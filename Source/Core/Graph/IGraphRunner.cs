namespace DotNetToolbox.AI.Graph;

public interface IGraphRunner : IDisposable {
    INode? CurrentNode { get; }
    Map State { get; }

    void Run(Map? initialState = null);
}
