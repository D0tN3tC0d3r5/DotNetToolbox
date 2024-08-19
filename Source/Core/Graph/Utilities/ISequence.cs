namespace DotNetToolbox.Graph.Utilities;

public interface ISequence<TValue>
    : IEnumerator<TValue>
    where TValue : notnull {
    TValue First { get; }
    new TValue Current { get; }
    TValue Next { get; set; }

    bool HasNext();
    TValue PeekNext();
}
