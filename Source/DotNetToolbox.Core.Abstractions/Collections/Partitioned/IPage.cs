namespace System.Collections;

public interface IPage<out TItem> : IPartition<TItem, uint>
{
    uint TotalCount { get; init; }
}