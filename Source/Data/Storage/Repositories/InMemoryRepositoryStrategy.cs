namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepositoryStrategy<TItem>(IItemSet<TItem> repository)
    : IRepositoryStrategy<InMemoryRepositoryStrategy<TItem>> {
    protected ItemSet<TItem> Repository { get; } = (ItemSet<TItem>)repository;

    public TResult ExecuteQuery<TResult>(LambdaExpression expression)
        => throw new NotImplementedException();

    public IItemSet Create(LambdaExpression expression)
        => ItemSet.Create(expression.Type, expression, this);
    // ReSharper disable once SuspiciousTypeConversion.Global
    public IItemSet<TResult> Create<TResult>(LambdaExpression expression)
        => (IItemSet<TResult>)ItemSet.Create(typeof(TResult), expression);

    public TResult ExecuteFunction<TResult>(string command, Expression? expression = null, object? input = null) {
        var source = expression is LambdaExpression lambda ? Repository.Create(lambda) : Repository.Data;
        object? result = command switch {
            "Any" when typeof(TResult) == typeof(bool) => source.Cast<object?>().Any(),
            "Count" when Repository.Data is ICollection<TItem> c && typeof(TResult) == typeof(int) => source.Cast<object?>().Count(),
            "Create" when typeof(TItem).IsClass && Repository.Data is ICollection<TItem> c && input is Action<TItem> set && typeof(TResult) == typeof(TItem) => CreateSetAndAddItem(c, set),
            "ElementAt" when input is int index => source.Cast<TItem>().ElementAt(index),
            "FindFirst" when typeof(TResult) == typeof(TItem) => Repository.Data.FirstOrDefault(),
            "GetList" when typeof(TResult) == typeof(TItem[]) => Repository.Data.ToArray(),
            _ => throw new NotSupportedException($"Command '{command}' is not supported for a '{Repository.GetType().Name}<{typeof(TItem).Name}>'."),
        };
        return IsOfTypeOrDefault<TResult>(result)!;
    }

    public void ExecuteAction(string command, Expression? expression = null, object? input = null) {
        var source = expression is LambdaExpression lambda ? Repository.Create(lambda) : Repository.Data;
        switch (command) {
            case "Add" when Repository.Data is ICollection<TItem> c && input is TItem i:
                c.Add(i);
                return;
        }
        throw new NotSupportedException($"Command '{command}' is not supported.");
    }

    private static TItem CreateSetAndAddItem(ICollection<TItem> collection, Action<TItem> set) {
        var item = Activator.CreateInstance<TItem>();
        set(item);
        collection.Add(item);
        return item;
    }
}
