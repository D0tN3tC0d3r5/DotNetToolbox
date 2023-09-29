namespace System.Collections.Partitioned;

public record Page<TItem>
    : IPage<TItem>
{
    public Page()
    {
    }

    public Page(IReadOnlyList<TItem> items, uint offset = 0, uint size = PartitionSettings.DefaultPartitionSize, uint totalCount = 0)
    {
        Size = size;
        Offset = offset;
        Items = IsNotNull(items);
        TotalCount = totalCount;
    }

    public Page(IPage<TItem> page, uint? totalCount = null)
        : this(page.Items, page.Offset, page.Size, totalCount ?? page.TotalCount)
    {
    }

    public uint Size { get; init; } = PartitionSettings.DefaultPartitionSize;
    public uint Offset { get; init; } = 0;
    public IReadOnlyList<TItem> Items { get; init; } = Array.Empty<TItem>();
    public uint TotalCount { get; init; }
}
