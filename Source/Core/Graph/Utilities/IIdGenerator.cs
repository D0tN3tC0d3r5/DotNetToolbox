namespace DotNetToolbox.Graph.Utilities;

public interface IIdGenerator<TKey> {
    TKey First { get; }
    TKey Peek();
    TKey Consume();
    void Set(TKey next);
    void Reset(TKey first);
}
