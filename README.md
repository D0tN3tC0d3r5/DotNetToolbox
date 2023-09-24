# DotNetToolbox

## Version 1.0.0

DotNetToolboxe is a comprehensive utility package that offers a wide range of functionalities, including pagination, date-time management, file system handling, result management, Azure secrets, Base64 GUIDs, and more.

### Components

#### Pagination

- `PagedCollection`: Manages paged collections.
- `Pagination`: Handles pagination logic.
- `PaginationSettings`: Stores pagination settings.

#### Date and Time

- `DateTimeProvider`: Offers functionalities for date-time management.

#### File System

- `FileSystemHandler`: Handles file manipulation tasks.

#### Results and Validation

- `CrudResult`: Represents the outcomes of CRUD operations.
- `HttpResult`: Represents the outcomes of HTTP requests.
- `Result`: Base class for operation results.
- `SignInResult`: Represents the outcomes of sign-in operations.
- `ValidationResult`: Represents the outcomes of validation checks.
- `ValidationError`: Focuses on validation-related errors.
- `ValidationErrorExtensions`: Extends functionalities for validation errors.

#### Azure

- `AzureSecretReader`: Facilitates reading secrets from Azure.

#### GUIDs

- `Base64Guid`: Handles Base64 encoding and decoding of GUIDs.

#### Object Creation and Validation

- `Create`: Assists in object creation.
- `Ensure`: Aids in object validation.

#### Extensions

- `ClientFactoryExtensions`: Provides extensions for client factories.
- `ConcurrentDictionaryExtensions`: Offers utility functions for concurrent dictionaries.
- `EnumerableExtensions`: Extends functionalities for enumerable types.

## Usage

Please refer to the API documentation for detailed usage instructions.

## Contributing

This package is intended for internal use. Contributions are restricted to team members.

## License

[MIT License](LICENSE)