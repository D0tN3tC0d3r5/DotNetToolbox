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
        var source = Repository.Data.AsQueryable();
        object? result = command switch {
            "Any" when expression is not null => Repository.Data.Any(ToDelegate<Func<TItem, bool>>(expression)),
            "Any" => Repository.Data.Any(),
            "Count" when expression is not null => Repository.Data.Count(ToDelegate<Func<TItem, bool>>(expression)),
            "Count" => Repository.Data.Count(),
            "FindFirst" when expression is not null => Repository.Data.FirstOrDefault(ToDelegate<Func<TItem, bool>>(expression)),
            "FindFirst" => Repository.Data.FirstOrDefault(),

            "GetList" => Repository.Data.ToArray(),
            "ElementAt" when input is int index => Repository.Data.ElementAt(index),

            "Create" when typeof(TItem).IsClass && input is Action<TItem> set => CreateSetAndAddItem(set),
            _ => throw new NotImplementedException($"Command '{command}' is not implemented for '{Repository.GetType().Name}<{typeof(TItem).Name}>'."),
        };
        return IsOfTypeOrDefault<TResult>(result)!;
    }

    private TResult ToDelegate<TResult>(Expression expression)
        where TResult : Delegate {
        try {
            if (expression is LambdaExpression lambda)
                return (TResult)lambda.Compile(true);
            throw new NotImplementedException($"Command expression '{expression}' is not implemented for '{Repository.GetType().Name}<{typeof(TItem).Name}>'.");
        } catch (Exception ex) {
            throw new NotSupportedException($"Command expression '{expression}' is not supported for '{Repository.GetType().Name}<{typeof(TItem).Name}>'.", ex);
        }
    }

    public void ExecuteAction(string command, Expression? expression = null, object? input = null) {
        var source = expression is LambdaExpression lambda ? Repository.Create(lambda) : Repository.Data;
        switch (command) {
            case "Add" when input is TItem i:
                DataAsCollection.Add(i);
                return;
        }
        throw new NotSupportedException($"Command '{command}' is not supported.");
    }

    private ICollection<TItem> DataAsCollection
        => Repository.Data as ICollection<TItem>
        ?? throw new NotSupportedException("Repository data is not a collection.");

    private TItem CreateSetAndAddItem(Action<TItem> set) {
        var item = Activator.CreateInstance<TItem>();
        set(item);
        DataAsCollection.Add(item);
        return item;
    }
}
