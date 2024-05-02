using static DotNetToolbox.Pagination.PaginationSettings;

namespace DotNetToolbox.Pagination;

public class Block<TItem>
    : IBlock<TItem> {
    [SetsRequiredMembers]
    public Block() : this([]) {
    }

    [SetsRequiredMembers]
    public Block(IReadOnlyList<TItem> items, uint size = DefaultBlockSize) {
        Items = IsNotNull(items);
        Size = size;
    }

    public required IReadOnlyList<TItem> Items { get; init; }
    public uint Size { get; init; }
}

public class Block<TItem, TMarker>
    : Block<TItem>
    , IBlock<TItem, TMarker> {
    [SetsRequiredMembers]
    public Block()
        : this([], default!) {
    }

    [SetsRequiredMembers]
    public Block(IReadOnlyList<TItem> items, TMarker marker, uint size = DefaultBlockSize)
        : base(items, size) {
        Marker = marker;
    }

    public required TMarker? Marker { get; init; }
}
