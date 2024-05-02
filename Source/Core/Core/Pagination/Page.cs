using static DotNetToolbox.Pagination.PaginationSettings;

namespace DotNetToolbox.Pagination;

public class Page<TItem>
    : IPage<TItem> {
    [SetsRequiredMembers]
    public Page() : this([]) {
    }

    [SetsRequiredMembers]
    public Page(IReadOnlyList<TItem> items, uint index = 0, uint size = DefaultPageSize, uint totalCount = 0) {
        Items = IsNotNull(items);
        Size = size;
        Index = index;
        TotalCount = totalCount;
    }

    public uint TotalCount { get; init; }
    public required IReadOnlyList<TItem> Items { get; init; }
    public uint Index { get; init; }
    public uint Size { get; init; }
}
