# DotNetToolbox.Data

## Introduction
DotNetToolbox.Data is a powerful C# library for .NET 8, designed to simplify data access and manipulation. It provides a range of utilities and patterns to enhance development by offering robust data handling capabilities and improving code testability.

## Installation
```shell
dotnet add package DotNetToolbox.Data
```

## Dependencies
- .NET 8
- DotNetToolbox.Core (automatically included)

## Core Concepts

### Result Pattern
The Result pattern is implemented through various classes to handle operation outcomes:

- `Result`: A base class for operation results.
- `CrudResult`: Specialized for CRUD operations.
- `HttpResult`: Tailored for HTTP responses in web APIs.
- `SignInResult`: Designed for authentication processes.

These classes provide a consistent way to handle success, failure, and various error states across different types of operations.

### Repository Pattern
The repository pattern is implemented through several interfaces and classes:

- `IRepository<TItem>`: Defines the base contract for repositories.
- `IReadOnlyRepository<TItem>`: For read-only operations.
- `IUpdatableRepository<TItem>`: For update operations.
- `IQueryableRepository<TItem>`: For queryable repositories.

The `Repository<TStrategy, TItem>` class provides a flexible implementation of these interfaces, allowing for different strategies to be applied.

### Key Handling
Key handling is managed through the `IKeyHandler<TKey>` interface and its implementations:

- `InMemoryKeyHandler<TKey>`: Base class for in-memory key handling.
- Specialized handlers for different key types (Guid, Int, Long, String, DateTime).

## System Utilities
The library includes several system utilities to abstract common operations:

- `DateTimeProvider`: For date and time operations.
- `GuidProvider`: For GUID generation.
- `FileSystem`: For file system operations.
- `Input` and `Output`: For console I/O operations.

## Data Access Utilities

### Repositories
- `InMemoryRepository<TItem, TKey>`: An in-memory implementation of the repository pattern.
- `QueryableRepository<TItem>`: Base class for queryable repositories.

### Strategies
- `RepositoryStrategy<TStrategy, TItem>`: Base class for implementing custom repository strategies.
- `InMemoryRepositoryStrategy<TItem>`: Strategy for in-memory data storage.

### Data Map
- `IDataContext`: Interface for data context operations like ensuring existence, seeding, and saving changes.

## Extension Methods
The library provides several extension methods to enhance functionality:

- `QueryableExtensions`: Extensions for `IQueryable<T>`.
- `EnumerableExtensions`: Extensions for `IEnumerable<T>`.

## Examples

### Using InMemoryRepository
```csharp
public class User : IEntity<int> {
    public int Key { get; set; }
    public string Name { get; set; }
}

var repository = new InMemoryRepository<User, int>();

// Adding a user
var newUser = new User { Name = "John Doe" };
repository.Add(newUser);

// Querying users
var user = repository.Find(u => u.Name == "John Doe");
```

### Implementing Custom Repository Strategy
```csharp
public class CustomStrategy<TItem> : RepositoryStrategy<CustomStrategy<TItem>, TItem> {
    public override void Add(TItem newItem) {
        // Custom implementation
    }
    
    // Implement other methods...
}

var repository = new Repository<CustomStrategy<User>, User>();
```

### Using CrudResult
```csharp
public CrudResult<User> GetUser(int id) {
    var user = repository.FindByKey(id);
    if (user == null) {
        return CrudResult<User>.NotFound();
    }
    return CrudResult<User>.Success(user);
}
```

## Contributing
Contributions to DotNetToolbox.Data are welcome. Please ensure that your code adheres to the project's coding standards and is covered by unit tests.

## License
This project is licensed under the MIT License - see the LICENSE file for details.