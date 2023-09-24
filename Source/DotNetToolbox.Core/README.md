# DotNetToolbox Core Library

## Version 1.0.0

The DotNetToolbox Core Library (DotNetToolbox.Core) is a comprehensive utility package that offers a wide range of functionalities, including pagination, date-time management, file system handling, result management, Azure secrets, Base64 GUIDs, and more.

### Components

#### Pagination

- `PagedCollection`: Manages paged collections.
- `Pagination`: Handles pagination logic.
- `PaginationSettings`: Stores pagination settings.

#### Date and Time

- `DateTimeProvider`: Offers functionalities for date-time management.

#### File System

- `FileSystemHandler`: Handles file manipulation tasks.

#### Results

- `CrudResult`: Represents the outcomes of CRUD operations.
- `HttpResult`: Represents the outcomes of HTTP requests.
- `Result`: Base class for operation results.
- `SignInResult`: Represents the outcomes of sign-in operations.
- `ValidationResult`: Represents the outcomes of validation checks.

#### Validation

- `ValidationError`: Focuses on validation-related errors.

#### Azure

- `AzureSecretReader`: Facilitates reading secrets from Azure.

#### Azure Active Directory

- `AzureActiveDirectoryOptions`: Manages configurations related to Azure AD.

#### GUIDs

- `Base64Guid`: Handles Base64 encoding and decoding of GUIDs.

#### Object Creation and Validation

- `Create`: Assists in object creation.
- `Ensure`: Aids in object validation.

#### Extensions

- `HttpClientFactoryExtensions`: Provides extensions for client factories.
- `ConcurrentDictionaryExtensions`: Offers utility functions for concurrent dictionaries.
- `EnumerableExtensions`: Extends functionalities for enumerable types.

#### HTTP Client Options

- `ConfidentialHttpClientOptions`: Defines options for confidential HTTP clients.
- `HttpClientOptions`: General options for HTTP clients.
- `IdentifiedHttpClientOptions`: Defines options for identified HTTP clients.

## Usage

Please refer to the API documentation for detailed usage instructions.

## Contributing

This package is intended for internal use. Contributions are restricted to team members.

## License

[MIT License](LICENSE)
