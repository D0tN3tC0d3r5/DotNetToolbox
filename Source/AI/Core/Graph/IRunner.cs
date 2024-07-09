namespace DotNetToolbox.AI.Graph;

public interface IRunner : IDisposable {
    INode? CurrentNode { get; }
    Map State { get; }

    void Reset();
    void Run();
}
