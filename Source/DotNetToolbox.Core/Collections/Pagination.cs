using static DotNetToolbox.Collections.PaginationSettings;

namespace DotNetToolbox.Collections;

public sealed record Pagination : HasDefault<Pagination>
{
    public Pagination() : this(0)
    {
    }

    public Pagination(uint pageIndex, uint? pageSize = null, uint totalCount = MaxCount)
    {
        PageSize = pageSize is null ? DefaultPageSize : Math.Min(pageSize.Value, MaxSize);
        var count = Math.Min(MaxCount, totalCount);
        var lastIndex = count == 0 || pageSize == 0 ? 0 : Math.Min(MaxIndex, (count / PageSize) - 1);
        PageIndex = Math.Min(lastIndex, pageIndex);
    }

    public uint PageIndex { get; }
    public uint PageSize { get; }
}
