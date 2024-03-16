## TestUtilities (DotNetToolbox.TestUtilities)

### Introduction
DotNetToolbox.TestUtilities is a comprehensive C# library for .NET 8, designed to enhance logging and tracking capabilities in testing environments. It provides robust tools for inspecting, verifying, and validating log outputs. The library includes classes that can be used as mocked instances in test cases where a test subject requires `ILogger`, `ILoggerFactory`, and `ILoggerProvider` to be injected. This makes it particularly useful for unit testing scenarios where you need to verify logging behavior or when testing components that interact with logging services.

### Table of Contents
1. [Installation](#installation)
2. [Dependencies](#dependencies)
3. [Features](#features)
4. [Usage](#usage)
5. [Examples](#examples)
6. [Contributors](#contributors)
7. [License](#license)

### Installation
```shell
PM> Install-Package DotNetToolbox.TestUtilities
```

### Dependencies
- .NET 8

### Features
- **ITrackedLogger**: Interface for loggers that track and store log entries.
- **Log**: Data structure for representing individual log entries.
- **TrackedLogger**: Extends basic logging to include tracking of log messages.
- **TrackedNullLogger**: An in-memory only logger for capturing and inspecting logs during tests.
- **TrackedLoggerFactory**: Factory for creating instances of `TrackedLogger`.
- **TrackedNullLoggerFactory**: Factory for creating instances of `TrackedNullLogger`.
- **TrackedLoggerProvider**: Provides `TrackedLogger` instances, integrating with existing logging systems.

### Usage
To use DotNetToolbox.TestUtilities, include it in your test project and instantiate the desired logging components, such as `TrackedNullLogger` or `TrackedLogger`, as per your testing requirements.

### Examples
Here are some examples demonstrating the usage of the library:

1. **Implementing TrackedLogger**:
   ```csharp
   var trackedLogger = new TrackedLogger<MyClass>(logger);
   trackedLogger.Log(LogLevel.Warning, new EventId(2, "WarningEvent"), "Warning message", null, Formatter);
   ```

2. **Creating Log Entries**:
   ```csharp
   var logEntry = new Log(LogLevel.Error, new EventId(3, "ErrorEvent"), "Error message", new Exception("TestException"), "Formatted message");
   ```

3. **Using TrackedNullLogger**:
   ```csharp
   var logger = TrackedNullLogger<MyClass>.Instance;
   logger.Log(LogLevel.Information, new EventId(1, "TestEvent"), "Test message", null, Formatter);
   ```

4. **Mocking ILoggerFactory for a Class Constructor**:
   ```csharp
   public class MyClass {
       private readonly ILogger _logger;

       public MyClass(ILoggerFactory loggerFactory) {
           _logger = loggerFactory.CreateLogger<MyClass>();
       }

       // Class methods
   }

   // In your test
   var loggerFactory = new TrackedLoggerFactory(Substitute.For<ILoggerFactory>());
   var myClassInstance = new MyClass(loggerFactory);
   ```
