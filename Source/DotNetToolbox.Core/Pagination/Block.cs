namespace System.Pagination;

public class Block<TItem>
    : Block<TItem, TItem>,
      IBlock<TItem> {

    public Block() {
    }

    public Block(IReadOnlyList<TItem> items, TItem offset, uint size = BlockSettings.DefaultBlockSize)
        : base(items, offset, size) {
    }
}

public class Block<TItem, TOffset>
    : IBlock<TItem, TOffset> {

    public Block() {
    }

    public Block(IReadOnlyList<TItem> items, TOffset offset, uint size = BlockSettings.DefaultBlockSize)
        : this() {
        Items = IsNotNull(items);
        Offset = offset;
        Size = size;
    }

    public IReadOnlyList<TItem> Items { get; init; } = Array.Empty<TItem>();
    public TOffset Offset { get; init; } = default!;
    public uint Size { get; init; } = BlockSettings.DefaultBlockSize;
}
