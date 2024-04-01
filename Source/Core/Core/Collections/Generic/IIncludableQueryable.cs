namespace DotNetToolbox.Collections.Generic;

public interface IIncludableQueryable<out TEntity, out TProperty> : IQueryable<TEntity>;