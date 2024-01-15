namespace DotNetToolbox.Pagination;

public class Block<TItem>
    : Block<TItem, TItem>,
      IBlock<TItem> {
    [SetsRequiredMembers]
    public Block() {
    }

    [SetsRequiredMembers]
    public Block(IReadOnlyList<TItem> items, TItem offset, uint size = BlockSettings.DefaultBlockSize)
        : base(items, offset, size) {
    }
}

public class Block<TItem, TOffset>
    : IBlock<TItem, TOffset> {
    [SetsRequiredMembers]
    public Block() {
        Items = Array.Empty<TItem>();
        Offset = default!;
    }

    [SetsRequiredMembers]
    public Block(IReadOnlyList<TItem> items, TOffset offset, uint size = BlockSettings.DefaultBlockSize) {
        Items = IsNotNull(items);
        Offset = offset;
        Size = size;
    }

    public required IReadOnlyList<TItem> Items { get; init; }
    public required TOffset Offset { get; init; }
    public uint Size { get; init; } = BlockSettings.DefaultBlockSize;
}
