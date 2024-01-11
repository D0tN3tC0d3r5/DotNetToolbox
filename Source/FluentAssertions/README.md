## TestUtilities (DotNetToolbox.TestUtilities)

### Introduction
`DotNetToolbox.FluentAssertions` is an extension of FluentAssertions, tailored for .NET 8, aimed at providing specialized assertion capabilities for logging components in .NET applications. It enhances test readability and maintainability by offering clear and concise assertion methods.

### Table of Contents
1. [Installation](#installation)
2. [Dependencies](#dependencies)
3. [Features](#features)
4. [Usage](#usage)
5. [Configuration](#configuration)
6. [Contributors](#contributors)
7. [License](#license)

### Installation
```shell
PM> Install-Package DotNetToolbox.FluentAssertions
```

### Dependencies
- DotNetToolbox.TestUtilities >= 8.0.3
- FluentAssertions >= 6.12.0

### Features
- **LoggerAssertions**: Custom assertions for testing logging behavior.
- **LoggerExtensions**: Extension methods for FluentAssertions to work seamlessly with `ILogger` instances.

### Usage
The library can be used in unit tests to assert various states and behaviors of loggers in .NET applications. Here are some examples:
```csharp
    logger.Should().BeEmpty();
    logger.Should().HaveExactly(2);
    logger.Contain(LogLevel.Information);
    logger.Contain("Test 1.");
    logger.ContainExactly(2, LogLevel.Error, "Error message.");
```

### Configuration
No additional configuration is required beyond the standard setup for FluentAssertions.
