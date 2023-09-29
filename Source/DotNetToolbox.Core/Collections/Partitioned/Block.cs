namespace System.Collections.Partitioned;

public record Block<TItem>
    : IBlock<TItem>
{
    public Block()
    {
    }

    public Block(IReadOnlyList<TItem> items, string? offset = null, uint size = PartitionSettings.DefaultPartitionSize)
    {
        Size = size;
        Offset = offset ?? string.Empty;
        Items = IsNotNull(items);
    }

    public Block(IBlock<TItem> page)
        : this(page.Items, page.Offset, page.Size)
    {
    }

    public uint Size { get; init; } = PartitionSettings.DefaultPartitionSize;
    public string Offset { get; init; } = string.Empty;
    public IReadOnlyList<TItem> Items { get; init; } = Array.Empty<TItem>();
}
