namespace DotNetToolbox.Graph;

public interface IGraph : IDisposable {
    INode? CurrentNode { get; }
    Map State { get; }

    void Run(Map? initialState = null);
}
