namespace DotNetToolbox.Pagination;

public interface IBlock<out TItem> {
    uint Size { get; }
    IReadOnlyList<TItem> Items { get; }
};

public interface IBlock<out TItem, out TMarker>
    : IBlock<TItem> {
    TMarker? Marker { get; }
}
