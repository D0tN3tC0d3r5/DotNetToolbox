namespace DotNetToolbox.Pagination;

public class Page<TItem>
    : Block<TItem, uint>,
      IPage<TItem> {
    [SetsRequiredMembers]
    public Page() {
    }

    [SetsRequiredMembers]
    public Page(IReadOnlyList<TItem> items, uint offset = 0, uint size = BlockSettings.DefaultBlockSize, uint totalCount = 0)
        : base(items, offset, size) {
        TotalCount = totalCount;
    }

    public uint TotalCount { get; init; }
}
