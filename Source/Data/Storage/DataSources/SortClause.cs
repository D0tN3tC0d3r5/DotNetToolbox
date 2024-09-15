namespace DotNetToolbox.Data.DataSources;

public record SortClause(string PropertyName, Type PropertyType, SortDirection Direction);

public record SortClause<TValue>(string PropertyName, SortDirection Direction) : SortClause(PropertyName, typeof(TValue), Direction);
