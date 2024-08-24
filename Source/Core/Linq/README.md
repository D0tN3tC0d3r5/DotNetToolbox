# DotNetToolbox.Linq

## Introduction
DotNetToolbox.Linq is a powerful C# library for .NET 8, designed to enhance and extend LINQ (Language Integrated Query) capabilities. It provides a suite of utilities and extension methods to simplify complex query operations, improve performance, and add new functionalities to LINQ.

## Installation
```shell
dotnet add package DotNetToolbox.Linq
```

## Dependencies
- .NET 8

## Key Features

### AsyncQueryable Extensions
- Provides asynchronous versions of common LINQ operations
- Supports efficient handling of large datasets in asynchronous scenarios

### Enhanced Queryable Extensions
- Extends IQueryable with additional methods for advanced querying
- Includes operations like `ToIndexedList`, `AsIndexed`, and more

### Expression Manipulation
- Utilities for modifying and transforming LINQ expressions
- Includes `ExpressionConversionVisitor` for type-safe expression modifications

### Type Mapping
- `TypeMapper` class for mapping between different types in LINQ expressions

## Core Components

### AsyncQueryable
- `IAsyncQueryable<T>`: Interface for asynchronous queryables
- `AsyncQueryable<T>`: Base implementation of IAsyncQueryable
- `EmptyAsyncQueryable<T>`: Represents an empty async queryable

### AsyncQueryableExtensions
Provides asynchronous versions of common LINQ operations, including:
- `AggregateAsync`
- `AllAsync`
- `AnyAsync`
- `AverageAsync`
- `ContainsAsync`
- `CountAsync`
- `ElementAtAsync`
- `FirstAsync`/`FirstOrDefaultAsync`
- `LastAsync`/`LastOrDefaultAsync`
- `MaxAsync`/`MinAsync`
- `SumAsync`
- `ToArrayAsync`/`ToListAsync`/`ToDictionaryAsync`/`ToHashSetAsync`

### QueryableExtensions
Extends IQueryable with additional methods:
- `AsIndexed`: Converts a query to an indexed sequence
- `ForEach`: Allows iteration over query results
- `ToIndexedList`: Creates a list of indexed items
- `ToArray`/`ToList`/`ToHashSet` with custom projections

### ExpressionExtensions
- `ReplaceExpressionType`: Allows type-safe modification of LINQ expressions
- `Apply`: Applies an expression to an enumerable collection

## Usage Examples

### Using AsyncQueryable
```csharp
IAsyncQueryable<int> asyncNumbers = AsyncQueryable.Range(1, 100);
var sum = await asyncNumbers.SumAsync();
```

### Asynchronous LINQ Operations
```csharp
var query = dbContext.Users.Where(u => u.IsActive);
var activeUserCount = await query.CountAsync();
var firstActiveUser = await query.FirstOrDefaultAsync();
```

### Using Indexed Queries
```csharp
var indexedQuery = dbContext.Products.AsIndexed();
var indexedList = await indexedQuery.ToListAsync();
foreach (var item in indexedList)
{
    Console.WriteLine($"Index: {item.Index}, Product: {item.Value.Name}");
}
```

### Expression Type Replacement
```csharp
Expression<Func<User, bool>> expr = u => u.Age > 18;
var convertedExpr = expr.ReplaceExpressionType(new TypeMapper<User, Customer>(u => new Customer { Age = u.Age }));
```

## Contributing
Contributions to DotNetToolbox.Linq are welcome. Please ensure that your code adheres to the project's coding standards and is covered by unit tests.

## License
This project is licensed under the MIT License - see the LICENSE file for details.