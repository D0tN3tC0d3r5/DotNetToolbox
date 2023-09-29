namespace System.Collections;

public interface IPartition<out TItem, out TOffset>
{
    uint Size { get; }
    TOffset Offset { get; }
    IReadOnlyList<TItem> Items { get; }
}