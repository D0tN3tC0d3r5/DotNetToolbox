namespace DotNetToolbox.Graph.Utilities;

public interface IIdGenerator<TKey> {
    TKey First { get; }
    TKey Next { get; }
    TKey PeekNext();
    TKey ConsumeNext();
    void SetNext(TKey next);
    void Reset(TKey first);
}
