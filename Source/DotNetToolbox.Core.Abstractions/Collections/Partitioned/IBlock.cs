namespace System.Collections;

public interface IBlock<out TItem> : IPartition<TItem, string>
{
}