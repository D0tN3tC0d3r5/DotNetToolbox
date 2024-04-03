// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace DotNetToolbox.Linq;

public interface IIncludableQueryable<out TEntity, out TProperty>
    : IQueryable<TEntity>;
