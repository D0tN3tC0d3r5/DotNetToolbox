
namespace DotNetToolbox.Data.Repositories;

internal sealed class InMemoryRepositoryStrategy<TRepository, TItem>(IEnumerable<TItem> source)
    : RepositoryStrategy<TItem>
    where TRepository : OrderedRepository<TRepository, TItem>
    where TItem : class {
    private readonly IQueryable<TItem> _source = source.AsQueryable();

    private static IRepository<TResult> ApplyAndCreate<TResult>(Func<IQueryable<TResult>> updateSource)
        where TResult : class {
        var result = updateSource();
        return RepositoryFactory.CreateRepository<TRepository, TResult>(result);
    }

    private static IOrderedRepository<TResult> ApplyAndCreateOrdered<TResult>(Func<IQueryable<TResult>> updateSource)
        where TResult : class {
        var result = updateSource();
        return RepositoryFactory.CreateOrderedRepository<TRepository, TResult>(result);
    }

    public override IRepository<TResult> OfType<TResult>()
        where TResult : class
        => ApplyAndCreate(_source.OfType<TResult>);

    public override IRepository<TResult> Cast<TResult>()
        where TResult : class
        => ApplyAndCreate(_source.Cast<TResult>);

    public override IRepository<TItem> Where(Expression<Func<TItem, bool>> predicate)
        => ApplyAndCreate(() => _source.Where(predicate));

    public override IRepository<TItem> Where(Expression<Func<TItem, int, bool>> predicate)
        => ApplyAndCreate(() => _source.Where(predicate));

    public override IRepository<TResult> Select<TResult>(Expression<Func<TItem, TResult>> selector)
        where TResult : class
        => ApplyAndCreate(() => _source.Select(selector));

    public override IRepository<TResult> Select<TResult>(Expression<Func<TItem, int, TResult>> selector)
        where TResult : class
        => ApplyAndCreate(() => _source.Select(selector));

    public override IRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, IEnumerable<TResult>>> selector)
        where TResult : class
        => ApplyAndCreate(() => _source.SelectMany(selector));

    public override IRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, int, IEnumerable<TResult>>> selector)
        where TResult : class
        => ApplyAndCreate(() => _source.SelectMany(selector));

    public override IRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class
        => ApplyAndCreate(() => _source.SelectMany(collectionSelector, resultSelector));

    public override IRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class
        => ApplyAndCreate(() => _source.SelectMany(collectionSelector, resultSelector));

    public override IRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, TInner, TResult>> resultSelector)
        where TResult : class
        => ApplyAndCreate(() => _source.Join(inner, outerKeySelector, innerKeySelector, resultSelector));

    public override IRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, TInner, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => ApplyAndCreate(() => _source.Join(inner, outerKeySelector, innerKeySelector, resultSelector, comparer));

    public override IRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector)
        where TResult : class
        => ApplyAndCreate(() => _source.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector));

    public override IRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => ApplyAndCreate(() => _source.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector, comparer));

    public override IOrderedRepository<TItem> Order()
        => ApplyAndCreateOrdered(_source.Order);

    public override IOrderedRepository<TItem> Order(IComparer<TItem> comparer)
        => ApplyAndCreateOrdered(() => _source.Order(comparer));

    public override IOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreateOrdered(() => _source.OrderBy(keySelector));

    public override IOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => ApplyAndCreateOrdered(() => _source.OrderBy(keySelector, comparer));

    public override IOrderedRepository<TItem> OrderDescending()
        => ApplyAndCreateOrdered(_source.OrderDescending);

    public override IOrderedRepository<TItem> OrderDescending(IComparer<TItem> comparer)
        => ApplyAndCreateOrdered(() => _source.OrderDescending(comparer));

    public override IOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreateOrdered(() => _source.OrderByDescending(keySelector));

    public override IOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => ApplyAndCreateOrdered(() => _source.OrderByDescending(keySelector, comparer));

    public override IOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => _source is not IOrderedQueryable<TItem> query
        ? throw new NotSupportedException("The collection must be ordered.")
        : ApplyAndCreateOrdered(() => query.ThenBy(keySelector));

    public override IOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => _source is not IOrderedQueryable<TItem> query
        ? throw new NotSupportedException("The collection must be ordered.")
        : ApplyAndCreateOrdered(() => query.ThenBy(keySelector, comparer));

    public override IOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => _source is not IOrderedQueryable<TItem> query
        ? throw new NotSupportedException("The collection must be ordered.")
        : ApplyAndCreateOrdered(() => query.ThenByDescending(keySelector));

    public override IOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => _source is not IOrderedQueryable<TItem> query
        ? throw new NotSupportedException("The collection must be ordered.")
        : ApplyAndCreateOrdered(() => query.ThenByDescending(keySelector, comparer));

    public override IRepository<TItem> Take(int count)
        => ApplyAndCreate(() => _source.Take(count));

    public override IRepository<TItem> Take(Range range)
        => ApplyAndCreate(() => _source.Take(range));

    public override IRepository<TItem> TakeWhile(Expression<Func<TItem, bool>> predicate)
        => ApplyAndCreate(() => _source.TakeWhile(predicate));

    public override IRepository<TItem> TakeWhile(Expression<Func<TItem, int, bool>> predicate)
        => ApplyAndCreate(() => _source.TakeWhile(predicate));

    public override IRepository<TItem> TakeLast(int count)
        => ApplyAndCreate(() => _source.TakeLast(count));

    public override IRepository<TItem> Skip(int count)
        => ApplyAndCreate(() => _source.Skip(count));

    public override IRepository<TItem> SkipWhile(Expression<Func<TItem, bool>> predicate)
        => ApplyAndCreate(() => _source.SkipWhile(predicate));

    public override IRepository<TItem> SkipWhile(Expression<Func<TItem, int, bool>> predicate)
        => ApplyAndCreate(() => _source.SkipWhile(predicate));

    public override IRepository<TItem> SkipLast(int count)
        => ApplyAndCreate(() => _source.SkipLast(count));

    public override IRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => _source.GroupBy(keySelector));

    public override IRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector)
        => ApplyAndCreate(() => _source.GroupBy(keySelector, elementSelector));

    public override IRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => _source.GroupBy(keySelector, comparer));

    public override IRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => _source.GroupBy(keySelector, elementSelector, comparer));

    public override IRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
        where TResult : class
        => ApplyAndCreate(() => _source.GroupBy(keySelector, elementSelector, resultSelector));

    public override IRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector)
        where TResult : class
        => ApplyAndCreate(() => _source.GroupBy(keySelector, resultSelector));

    public override IRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => ApplyAndCreate(() => _source.GroupBy(keySelector, resultSelector, comparer));

    public override IRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => ApplyAndCreate(() => _source.GroupBy(keySelector, elementSelector, resultSelector, comparer));

    public override IRepository<TItem> Distinct()
        => ApplyAndCreate(_source.Distinct);

    public override IRepository<TItem> Distinct(IEqualityComparer<TItem>? comparer)
        => ApplyAndCreate(() => _source.Distinct(comparer));

    public override IRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => _source.DistinctBy(keySelector));

    public override IRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => _source.DistinctBy(keySelector, comparer));

    public override IRepository<TItem[]> Chunk(int size)
        => ApplyAndCreate(() => _source.Chunk(size));

    public override IRepository<TItem> Concat(IEnumerable<TItem> source)
        => ApplyAndCreate(() => _source.Concat(source));

    public override IRepository<TResult> Combine<TSecond, TResult>(IEnumerable<TSecond> source2, Expression<Func<TItem, TSecond, TResult>> resultSelector)
        where TResult : class
        => ApplyAndCreate(() => _source.Zip(source2, resultSelector));

    public override IRepository<IPack<TItem, TSecond>> Zip<TSecond>(IEnumerable<TSecond> source) {
        Expression<Func<(TItem First, TSecond Second), IPack<TItem, TSecond>>> convert = z => new Pack<TItem, TSecond>(z.First, z.Second);
        return ApplyAndCreate(() => _source.Zip(source).Select(convert));
    }

    public override IRepository<IPack<TItem, TSecond, TThird>> Zip<TSecond, TThird>(IEnumerable<TSecond> source2, IEnumerable<TThird> source3) {
        Expression<Func<(TItem First, TSecond Second, TThird Third), IPack<TItem, TSecond, TThird>>> convert = z => new Pack<TItem, TSecond, TThird>(z.First, z.Second, z.Third);
        return ApplyAndCreate(() => _source.Zip(source2, source3).Select(convert));
    }

    public override IRepository<TItem> Union(IEnumerable<TItem> source2)
        => ApplyAndCreate(() => _source.Union(source2));

    public override IRepository<TItem> Union(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => ApplyAndCreate(() => _source.Union(source2, comparer));

    public override IRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => _source.UnionBy(source2, keySelector));

    public override IRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => _source.UnionBy(source2, keySelector, comparer));

    public override IRepository<TItem> Intersect(IEnumerable<TItem> source2)
        => ApplyAndCreate(() => _source.Intersect(source2));

    public override IRepository<TItem> Intersect(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => ApplyAndCreate(() => _source.Intersect(source2, comparer));

    public override IRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => _source.IntersectBy(source2, keySelector));

    public override IRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => _source.IntersectBy(source2, keySelector, comparer));

    public override IRepository<TItem> Except(IEnumerable<TItem> source2)
        => ApplyAndCreate(() => _source.Except(source2));

    public override IRepository<TItem> Except(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => ApplyAndCreate(() => _source.Except(source2, comparer));

    public override IRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => _source.ExceptBy(source2, keySelector));

    public override IRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => _source.ExceptBy(source2, keySelector, comparer));

    #pragma warning disable CS8634 // This warning is wrong. TItem has the class constraint.
    public override IRepository<TItem?> DefaultIfEmpty()
        => ApplyAndCreate(_source.DefaultIfEmpty);
    #pragma warning restore CS8634

    public override IRepository<TItem> DefaultIfEmpty(TItem defaultValue)
        => ApplyAndCreate(() => _source.DefaultIfEmpty(defaultValue));

    public override IRepository<TItem> Reverse()
        => ApplyAndCreate(_source.Reverse);

    public override IRepository<TItem> Append(TItem element)
        => ApplyAndCreate(() => _source.Append(element));

    public override IRepository<TItem> Prepend(TItem element)
        => ApplyAndCreate(() => _source.Prepend(element));
}

//public class InMemoryRepositoryStrategy<TModel, TEntity>(IEnumerable<TEntity> remote,
//                                                         Expression<Func<TModel, TEntity>> projectToEntity,
//                                                         Expression<Func<TEntity, TModel>> projectToModel)
//    : RepositoryQueryStrategy<TModel, TEntity>(remote, projectToEntity, projectToModel)
//    where TModel : class
//    where TEntity : class {
//    //public override bool HaveAny()
//    //    => Remote.Any();

//    //public override int Count()
//    //    => Remote.Count();

//    //public override TModel[] ToArray()
//    //    => [.. Remote.AsQueryable().Select(ProjectToModel)];

//    //public override TModel? GetFirst() {
//    //    var entity = Remote.FirstOrDefault();
//    //    return entity is null
//    //        ? default
//    //        : ConvertToModel(entity);
//    //}

//    //public override void Add(TModel newItem) {
//    //    var collection = IsOfType<ICollection<TEntity>>(Remote);
//    //    var newEntity = ConvertToEntity(newItem);
//    //    collection.Add(newEntity);
//    //}

//    //public override void Update(Expression<Func<TModel, bool>> predicate, TModel updatedItem) {
//    //    var collection = IsOfType<IList<TEntity>>(Remote);
//    //    var convertedPredicate = ConvertToRemoteExpression<Func<TEntity, bool>>(predicate);
//    //    var itemToUpdate = Remote.AsQueryable().FirstOrDefault(convertedPredicate);
//    //    if (itemToUpdate is null) return;
//    //    var index = collection.IndexOf(itemToUpdate);
//    //    collection[index] = ConvertToEntity(updatedItem);
//    //}

//    //public override void Remove(Expression<Func<TModel, bool>> predicate) {
//    //    var collection = IsOfType<ICollection<TEntity>>(Remote);
//    //    var convertedPredicate = ConvertToRemoteExpression<Func<TEntity, bool>>(predicate);
//    //    var itemToRemove = Remote.AsQueryable().FirstOrDefault(convertedPredicate);
//    //    if (itemToRemove is null) return;
//    //    collection.Remove(itemToRemove);
//    //}
//}
