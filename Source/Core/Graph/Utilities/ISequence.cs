namespace DotNetToolbox.Graph.Utilities;

public interface ISequence<TValue>
    : IEnumerator<TValue> {
    TValue First { get; init; }
    TValue Next { get; set; }

    bool HasNext();
    TValue PeekNext();
}
