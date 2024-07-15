using static DotNetToolbox.Pagination.PaginationSettings;

namespace DotNetToolbox.Pagination;

[method: SetsRequiredMembers]
public class Chunk<TItem>(IReadOnlyList<TItem> items, uint size = DefaultBlockSize)
    : IChunk<TItem> {
    [SetsRequiredMembers]
    public Chunk() : this([]) {
    }

    public required IReadOnlyList<TItem> Items { get; init; } = IsNotNull(items);
    public uint Size { get; init; } = size;
}

[method: SetsRequiredMembers]
public class Chunk<TItem, TMarker>(IReadOnlyList<TItem> items, TMarker? marker, uint size = DefaultBlockSize)
    : Chunk<TItem>(items, size)
    , IChunk<TItem, TMarker> {
    [SetsRequiredMembers]
    public Chunk()
        : this([], default!) {
    }

    public required TMarker? Marker { get; init; } = marker;
}
