namespace DotNetToolbox.Graph;

public interface IGraph : IDisposable {
    INode? CurrentNode { get; }
    Context State { get; }

    void Run(Context? initialState = null);
}
