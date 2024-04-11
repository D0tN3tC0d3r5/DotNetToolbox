namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepositoryStrategy<TItem>(IItemSet<TItem> repository)
    : IRepositoryStrategy<InMemoryRepositoryStrategy<TItem>> {
    protected ItemSet<TItem> Repository { get; } = (ItemSet<TItem>)repository;

    public TResult ExecuteQuery<TResult>(Expression expression, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public TResult ExecuteFunction<TResult>(string command, object? input = null) {
        object? result = command switch {
            "Count" when Repository.Data is ICollection<TItem> c && typeof(TResult) == typeof(int) => c.Count,
            "Any" when typeof(TResult) == typeof(bool) => Repository.Data.Any(),
            "FindFirst" when typeof(TResult) == typeof(TItem) => Repository.Data.First(),
            "GetList" when typeof(TResult) == typeof(TItem[]) => Repository.Data.ToArray(),
            "Create" when Repository.Data is ICollection<TItem> c && input is Action<TItem> set && typeof(TResult) == typeof(TItem) => CreateSetAndAddItem(c, set),
            _ => throw new NotSupportedException($"Command '{command}' is not supported for a '{Repository.GetType().Name}<{typeof(TItem).Name}>'."),
        };
        return IsOfType<TResult>(result);
    }

    private static TItem CreateSetAndAddItem(ICollection<TItem> collection, Action<TItem> set)
    {
        var item = Activator.CreateInstance<TItem>();
        set(item);
        collection.Add(item);
        return item;
    }

    public void ExecuteAction(string command, object? input = null) {
        switch (command) {
            case "Add" when Repository.Data is ICollection<TItem> c && input is TItem i:
                c.Add(i);
                return;
        }
        throw new NotSupportedException($"Command '{command}' is not supported.");
    }
}
