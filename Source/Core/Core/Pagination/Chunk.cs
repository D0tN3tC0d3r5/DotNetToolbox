using static DotNetToolbox.Pagination.PaginationSettings;

namespace DotNetToolbox.Pagination;

public class Chunk<TItem>
    : IChunk<TItem> {
    [SetsRequiredMembers]
    public Chunk() : this([]) {
    }

    [SetsRequiredMembers]
    public Chunk(IReadOnlyList<TItem> items, uint size = DefaultBlockSize) {
        Items = IsNotNull(items);
        Size = size;
    }

    public required IReadOnlyList<TItem> Items { get; init; }
    public uint Size { get; init; }
}

public class Chunk<TItem, TMarker>
    : Chunk<TItem>
    , IChunk<TItem, TMarker> {
    [SetsRequiredMembers]
    public Chunk()
        : this([], default!) {
    }

    [SetsRequiredMembers]
    public Chunk(IReadOnlyList<TItem> items, TMarker? marker, uint size = DefaultBlockSize)
        : base(items, size) {
        Marker = marker;
    }

    public required TMarker? Marker { get; init; }
}
