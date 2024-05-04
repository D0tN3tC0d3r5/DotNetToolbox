namespace DotNetToolbox.Pagination;

public interface IChunk<out TItem> {
    uint Size { get; }
    IReadOnlyList<TItem> Items { get; }
};

public interface IChunk<out TItem, out TMarker>
    : IChunk<TItem> {
    TMarker? Marker { get; }
}
