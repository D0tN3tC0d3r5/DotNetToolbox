namespace DotNetToolbox.Data.Repositories;

public interface ICanCreateItem<out TItem>
    where TItem : class {
    TItem Create(Action<TItem> setItem);
}
