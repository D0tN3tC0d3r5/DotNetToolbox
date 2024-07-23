namespace DotNetToolbox.Graph.Nodes;

public interface IKeyProvider<out TKey> {
    void Reset();
    TKey Peek();
    TKey GetNext();
}
