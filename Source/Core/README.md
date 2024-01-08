### DotNetToolbox.Core README

#### Introduction
DotNetToolbox.Core is a versatile C# library for .NET 8, designed to enhance development by providing a range of utilities and patterns. It simplifies complex tasks and improves code testability.

#### Table of Contents
1. [Result Patterns](#result-patterns)
2. [System Utilities](#system-utilities)
3. [Pagination Utilities](#pagination-utilities)
4. [Singleton and Options Patterns](#singleton-and-options-patterns)
5. [Extension Classes](#extension-classes)
6. [Other Utilities](#other-utilities)
7. [Installation](#installation)
8. [Dependencies](#dependencies)
9. [Contributing](#contributing)
10. [License](#license)

#### Result Patterns
- **ValidationError**: Core class for representing generic validation errors.

The `ValidationError` class represents individual validation errors, useful for detailed error tracking in data validation scenarios.

##### Examples:
1. **Creating a ValidationError:**
    ```csharp
    // Create a validation error with a source and message
    ValidationError emailError = new ValidationError("Email", "Invalid email format");
    Console.WriteLine(emailError);

    // Create a validation error with only a message
    ValidationError generalError = new ValidationError("General error occurred");
    Console.WriteLine(generalError);
    ```

2. **Using Implicit Conversion:**
    ```csharp
    ValidationError implicitError = "Username is required";
    Console.WriteLine(implicitError);
    ```

It has extension methods for collections of `ValidationError`s, such as checking if a specific error is contained within the collection.

3. **Check for an error in a collection:**
    ```csharp
    List<ValidationError> errors = new List<ValidationError> {
        "Email is required",
        new ValidationError("Password", "Password is too weak")
    };

    // Check if a specific error message exists in the collection
    bool hasEmailError = errors.Contains("Email", "Email is required");
    Console.WriteLine($"Email error present: {hasEmailError}");
    ```
 
    It also works with a custom exception class that encapsulates a collection of `ValidationError`s, intended to be thrown when validation fails.

4. **Check for an error in a collection:**
    ```csharp
    try {
        var result = ValidateUserData(userData);
        if (result.IsInvalid) {
            throw new ValidationException(result.Errors);
        }
    } catch (ValidationException ex) {
        foreach (var error in ex.Errors) {
            Console.WriteLine(error);
        }
    }
    ```

- **Result**: Core class for generic operation results. Used mostly for validation and error handling.

The `Result` class provides a flexible way to represent the outcome of operations, whether successful, invalid due to data issues, or errored due to exceptions.

##### Examples:
1. **Creating a Successful Result:**
    ```csharp
    var successResult = Result.Success();
    Console.WriteLine($"Operation Successful: {successResult.IsSuccess}");
    ```

2. **Creating a Result with Data Validation Error:**
    ```csharp
    var invalidResult = Result.InvalidData("Invalid input data");
    Console.WriteLine($"Data is Valid: {!invalidResult.IsInvalid}");
    ```

3. **Creating a Result Representing an Error:**
    ```csharp
    var errorResult = Result.Error(new Exception("Unexpected error occurred"));
    Console.WriteLine($"Operation Successful: {errorResult.IsSuccess}");
    ```

4. **Using Asynchronous Methods:**
    ```csharp
    async Task PerformOperationAsync() {
        var result = await Result.ErrorTask("Async operation failed");
        Console.WriteLine($"Operation Successful: {result.IsSuccess}");
    }
    ```

5. **Implicit Conversion from `ValidationError` or `Exception`:**
    ```csharp
    Result resultFromError = new ValidationError("Email", "Invalid email format");
    Result resultFromException = new Exception("Database connection failed");
    ```

The `Result<TValue>` class extends the functionality of the `Result` class, allowing you to include a value with the operation outcome. It is particularly useful when an operation needs to return a value on success. Below are examples of how to use the `Result<TValue>` class:

##### Examples:
1. **Returning a Value on Successful Operation:**
    ```csharp
    Result<int> CalculateSquare(int number) {
        int square = number * number;
        return Result.Success(square);
    }

    var squareResult = CalculateSquare(5);
    if (squareResult.IsSuccess) {
        Console.WriteLine($"Square of 5 is {squareResult.Value}");
    }
    ```

2. **Handling an Error with `Result<TValue>`:**
    ```csharp
    Result<string> FetchData(string url) {
        try {
            // Assuming GetData is a method that fetches data from a source
            string data = GetData(url); 
            return Result.Success(data);
        } catch (Exception ex) {
            return Result<string>.Error(ex);
        }
    }

    var apiResult = FetchDataFromApi("https://example.com/data");
    if (apiResult.IsSuccess) {
        Console.WriteLine($"Fetched data: {apiResult.Value}");
    } else {
        Console.WriteLine("Failed to fetch data");
    }
    ```

- **CrudResult.cs**: Specialized for CRUD operations.

The `CrudResult` class is designed for outcomes of CRUD operations, providing specific status types like `NotFound`, `Conflict`, etc. Here are some examples:

##### Examples:
1. **Indicating a Successful CRUD Operation:**
    ```csharp
    CrudResult CreateRecord(Record record) {
        // Logic to create a record
        return CrudResult.Success();
    }

    var creationResult = CreateRecord(new Record());
    if (creationResult.IsSuccess) {
        Console.WriteLine("Record created successfully.");
    }
    ```

2. **Handling a 'Not Found' Scenario in CRUD Operations:**
    ```csharp
    CrudResult DeleteRecord(int id) {
        // Logic to delete a record
        if (recordNotFound) {
            return CrudResult.NotFound();
        }
        return CrudResult.Success();
    }

    var deletionResult = DeleteRecord(5);
    if (deletionResult.WasNotFound) {
        Console.WriteLine("Record not found.");
    }
    ```

`CrudResult<TValue>` extends `CrudResult` to include a value with the operation outcome, useful for operations like 'Read'.

##### Examples:
1. **Returning a Value on Successful 'Read' Operation:**
    ```csharp
    CrudResult<Record> ReadRecord(int id) {
        Record? record = // Logic to read a record
        if (record == null) {
            return CrudResult<Record>.NotFound();
        }
        return CrudResult<Record>.Success(record);
    }

    var readResult = ReadRecord(3);
    if (readResult.IsSuccess) {
        Console.WriteLine($"Record read: {readResult.Value}");
    } else if (readResult.WasNotFound) {
        Console.WriteLine("Record not found.");
    }
    ```

2. **Handling a 'Conflict' in CRUD Operations:**
    ```csharp
    CrudResult<Record> UpdateRecord(Record updatedRecord) {
        // Logic to update a record
        if (recordHasConflict) {
            return CrudResult<Record>.Conflict();
        }
        return CrudResult<Record>.Success(updatedRecord);
    }

    var updateResult = UpdateRecord(new Record());
    if (updateResult.HasConflict) {
        Console.WriteLine("Record update conflict occurred.");
    }
    ```

- **HttpResult.cs**: Tailored for HTTP transactions.

The `HttpResult` class is designed to represent the outcome of HTTP operations, mapping closely to HTTP response statuses. Here are some examples:

##### Examples:
1. **Returning an 'Ok' HTTP Response:**
    ```csharp
    HttpResult GetUserData() {
        // Logic to get user data
        return HttpResult.Ok();
    }

    var response = GetUserData();
    if (response.IsOk) {
        Console.WriteLine("User data retrieved successfully.");
    }
    ```

2. **Handling a 'Bad Request' Scenario:**
    ```csharp
    HttpResult UpdateUserProfile(UserProfile profile) {
        if (!IsValid(profile)) {
            return HttpResult.BadRequest(Result.InvalidData("Invalid profile data"));
        }
        // Update logic
        return HttpResult.Ok();
    }

    var updateResponse = UpdateUserProfile(new UserProfile());
    if (updateResponse.IsBadRequest) {
        Console.WriteLine("Profile update failed due to invalid data.");
    }
    ```

`HttpResult<TValue>` extends `HttpResult` to include a value with the HTTP response, useful for GET requests or any operation returning data.

##### Examples:
1. **Returning Data on Successful Operation:**
    ```csharp
    HttpResult<UserProfile> GetUserProfile(int userId) {
        UserProfile? userProfile = // Fetch user profile logic
        if (userProfile == null) {
            return HttpResult<UserProfile>.NotFound();
        }
        return HttpResult<UserProfile>.Ok(userProfile);
    }

    var profileResponse = GetUserProfile(1);
    if (profileResponse.IsOk) {
        Console.WriteLine($"User Profile: {profileResponse.Value}");
    } else if (profileResponse.WasNotFound) {
        Console.WriteLine("User profile not found.");
    }
    ```

2. **Handling an Unauthorized Access:**
    ```csharp
    HttpResult<string> GetSecretData(string authToken) {
        if (!IsAuthorized(authToken)) {
            return HttpResult<string>.Unauthorized();
        }
        string secretData = FetchSecretData(); // Assume this fetches data
        return HttpResult<string>.Ok(secretData);
    }

    var secretDataResponse = GetSecretData("someAuthToken");
    if (secretDataResponse.IsUnauthorized) {
        Console.WriteLine("Unauthorized access.");
    }
    ```

- **SignInResult.cs**: Designed for authentication processes.

The `SignInResult` class is designed to encapsulate the outcome of authentication operations, providing specific statuses like `Locked`, `Blocked`, `Failed`, etc. Here are some examples:

##### Examples:
1. **Successful Sign-In:**
    ```csharp
    SignInResult AuthenticateUser(string username, string password) {
        // Logic to authenticate user
        if (isAuthenticated) {
            string token = GenerateToken(user); // Assume GenerateToken generates a token
            return new SignInResult(token);
        }
        return SignInResult.Failed();
    }

    var signInResult = AuthenticateUser("user1", "password123");
    if (signInResult.IsSuccess) {
        Console.WriteLine($"Authentication successful. Token: {signInResult.Token}");
    } else {
        Console.WriteLine("Authentication failed.");
    }
    ```

2. **Handling Locked Account:**
    ```csharp
    SignInResult CheckAccountStatus(User user) {
        if (user.IsLocked) {
            return SignInResult.Locked();
        }
        // Further checks
    }

    var accountStatus = CheckAccountStatus(someUser);
    if (accountStatus.IsLocked) {
        Console.WriteLine("Account is locked.");
    }
    ```

3. **Two-Factor Authentication Required:**
    ```csharp
    SignInResult PerformTwoFactorCheck(User user) {
        if (user.RequiresTwoFactor) {
            return SignInResult.TwoFactorRequired();
        }
        return SignInResult.Success(GenerateToken(user)); // Success with token
    }

    var twoFactorResult = PerformTwoFactorCheck(someUser);
    if (twoFactorResult.RequiresTwoFactor) {
        Console.WriteLine("Two-factor authentication required.");
    }
    ```

#### System Utilities
System utilities provide abstractions over system resources like Date and Time, GUIDs, File System, and Console Input/Output. These abstractions are instrumental in creating testable code by enabling dependency injection, which allows for the substitution of these system classes with mock objects during testing.

- **DateTimeProvider.cs**: Facilitates working with dates and times in a testable way by abstracting system-specific implementations.

##### Examples:
1. **Injecting DateTimeProvider into a service for testability:**
    ```csharp
    public class TimeSensitiveService {
        private readonly DateTimeProvider _dateTimeProvider;

        public TimeSensitiveService(DateTimeProvider dateTimeProvider) {
            _dateTimeProvider = dateTimeProvider;
        }

        public bool IsBusinessHour() {
            var currentHour = _dateTimeProvider.Now.Hour;
            return currentHour >= 9 && currentHour < 17;
        }
    }
    ```

2. **Mocking DateTimeProvider for unit tests:**
    ```csharp
    var fixedDateTime = Substitute.For<DateTimeProvider>();
    fixedDateTime.Now.Returns(new DateTimeOffset(2023, 1, 1, 12, 0, 0, TimeSpan.Zero));
    
    var service = new TimeSensitiveService(fixedDateTime);
    var result = service.IsBusinessHour();
    
    result.Should().BeTrue(); // FluentAssertions used here
    ```

- **GuidProvider.cs**: Allows generation of GUIDs that can be controlled in a testing environment.

##### Examples:
1. **Using GuidProvider to generate GUIDs in a service:**
    ```csharp
    public class IdentifierService {
        private readonly GuidProvider _guidProvider;

        public IdentifierService(GuidProvider guidProvider) {
            _guidProvider = guidProvider;
        }

        public Guid GetUniqueIdentifier() {
            return _guidProvider.New();
        }
    }
    ```

2. **Mocking GuidProvider in tests:**
    ```csharp
    var expectedGuid = Guid.NewGuid();
    var fixedGuidProvider = Substitute.For<GuidProvider>();
    fixedGuidProvider.New().Returns(expectedGuid);
    
    var service = new IdentifierService(fixedGuidProvider);
    var id = service.GetUniqueIdentifier();
    
    id.Should().Be(expectedGuid); // Asserting that the ID is indeed a GUID
    ```

- **FileSystem.cs**: Provides an abstraction over file system operations, enabling better testing of file-related functionality.

##### Examples:
1. **Injecting FileSystem into a component:**
    ```csharp
    public class FileManager {
        private readonly FileSystem _fileSystem;

        public FileManager(FileSystem fileSystem) {
            _fileSystem = fileSystem;
        }

        public bool FileExists(string path) {
            return _fileSystem.FileExists(path);
        }
    }
    ```

2. **Mocking FileSystem during testing:**
    ```csharp
    var mockedFileSystem = Substitute.For<FileSystem>();
    mockedFileSystem.FileExists("test.txt").Returns(true);
    
    var fileManager = new FileManager(mockedFileSystem);
    var exists = fileManager.FileExists("test.txt");
    
    exists.Should().BeTrue(); // Validate the expected behavior
    ```

- **Input.cs**, **Output.cs**: Utilities for input and output operations, making console interactions testable.

##### Examples:
1. **Using Output in a service to write messages:**
    ```csharp
    public class ConsoleService {
        private readonly Input _input;
        private readonly Output _output;

        public ConsoleService(Input input, Output output) {
            _input = input;
            _output = output;
        }

        public string ReadUserInputOrEmpty() {
            return _input.ReadLine() ?? string.Empty;
        }

        public void DisplayMessage(string target) {
            _output.WriteLine($"Hello {target}!");
        }
    }
    ```

2. **Testing console input and output interactions:**
    ```csharp
    var mockedOutput = Substitute.For<Output>();
    var mockedInput = Substitute.For<Input>();
    var consoleService = new ConsoleService(mockedInput, mockedOutput);
    mockedInput.ReadLine().Returns("Test")

    var target = consoleService.ReadUserInputOrEmpty();
    var consoleService.DisplayMessage(target);

    outputMock.Received().WriteLine("Hello Test!"); // NSubstitute used to verify interaction
    ```

#### Pagination Utilities
Pagination utilities provide a standardized way to handle the slicing of large datasets into smaller, more manageable blocks or pages. These utilities can be used to define and access specific segments of data, often necessary when implementing APIs that support pagination.

- **IBlock / Block**: Interfaces and classes that represent a contiguous block of items with a specific size and an offset that indicates the starting point or key item.

- **IPage / Page**: Extends the concept of a block to include pagination-specific information, such as the total count of items, allowing for the calculation of the total number of pages.

- **BlockSettings**: Provides constants related to pagination, such as default block size and maximum allowable values.

##### Examples:
1. **Creating a Block of items:**
   ```csharp
   var items = new List<string> { "Item1", "Item2", "Item3" };
   var block = new Block<string>(items, "Item1");

   // block now contains a block of items starting from "Item1"
   ```

2. **Creating a Page with total count information:**
   ```csharp
   var items = new List<string> { "Item1", "Item2", "Item3" };
   var page = new Page<string>(items, 0, BlockSettings.DefaultBlockSize, 100);

   // page now contains a page of items with information about the total count of items
   ```

#### Singleton and Options Patterns
Pattern utilities offer standardized ways to define and access special instances of classes, such as singleton, default, or empty instances. These utilities are designed to implement common design patterns in a consistent manner across your applications.

- **IHasInstance / HasInstance**: Allows for defining and accessing a singleton instance of a class.

- **IHasDefault / HasDefault**: Provides a way to access a default instance of a class.

- **IHasEmpty / HasEmpty**: Enables defining an 'empty' instance, often representing the absence of a value.

- **INamedOptions / NamedOptions**: Used for options classes with a standard way to define the section name used for configuration binding.

##### Examples:
1. **Accessing a singleton instance:**
    ```csharp
    public class SingletonService : HasInstance<SingletonService> {
        // Service implementation
    }

    var instance = SingletonService.Instance;
    ```

2. **Defining default options:**
    ```csharp
    public class ServiceOptions : NamedOptions<ServiceOptions> {
        public string ConnectionString { get; set; } = "DefaultConnectionString";
    }

    var defaultOptions = ServiceOptions.Default;
    ```

    Here is an example of the options class being used in a configuration binding scenario:

    ```json
    // In appsettings.json
    {
      // ...
      "Service": {
        "ConnectionString": "SomeConnectionString"
      }
      // ...
    }
    ```

    ```csharp
    public class Service {
        private readonly ServiceOptions _options;

        public Service(IOptions<ServiceOptions> options) {
            _options = options.Value;
        }

        public string GetConnectionString() {
            return _options.ConnectionString;
        }
    }

    // In Startup.cs
    services.Configure<ServiceOptions>(Configuration.GetSection("Service"));
    ```

3. **Using an empty instance:**
    ```csharp
    public class CommandResult : HasEmpty<CommandResult> {
        public bool Success { get; set; }
    }

    var noResult = CommandResult.Empty;
    ```

#### Extension Classes
Extension classes provide additional methods to existing types, enhancing their capabilities and simplifying common operations.

- **EnumerableExtensions.cs**: Extends `IEnumerable<T>` to provide additional transformation and collection generation methods.

##### Examples:
1. **Transforming IEnumerable to Array with custom projection:**
    ```csharp
    IEnumerable<int> numbers = new[] { 1, 2, 3, 4 };
    int[] squaredNumbers = numbers.ToArray(x => x * x);

    // squaredNumbers would be [1, 4, 9, 16]
    ```

2. **Creating a Dictionary from onother IDictionary with a value transformation:**
    ```csharp
    IDictionary<string, int>> original = new Dictionary<string, int> {
        ["one"] = 1,
        ["two"] = 2
    };
    var newDictionary = original.ToDictionary(v => v * 10); // Multiplies each value by 10

    // newDictionary would have values [10, 20]
    ```

- **QueryableExtensions.cs**: Provides projection and conversion extensions for `IQueryable<T>`.

##### Examples:
1. **Projecting IQueryable to Array:**
    ```csharp
    IQueryable<Person> people = GetPeopleQuery();
    PersonDto[] dtos = people.ToArray(person => new PersonDto(person.Name, person.Age));

    // dtos contains PersonDto objects projected from the Person query
    ```

2. **Transforming IQueryable into HashSet with projection:**
    ```csharp
    IQueryable<string> names = GetNamesQuery();
    HashSet<string> upperCaseNames = names.ToHashSet(name => name.ToUpper());

    // upperCaseNames contains upper case versions of the names from the query
    ```

- **TaskExtensions.cs**: Adds methods to `Task` and `ValueTask` to handle fire-and-forget scenarios with optional exception handling callbacks.

##### Examples:
1. **Using FireAndForget with a Task:**
    ```csharp
    Task someTask = DoWorkAsync();
    someTask.FireAndForget(); // No need to await, exceptions will be ignored
    ```

2. **Handling exceptions with FireAndForget:**
    ```csharp
    async Task DoWorkAsync() {
        // Simulate work
        await Task.Delay(1000);
        throw new InvalidOperationException("Something went wrong");
    }

    Task anotherTask = DoWorkAsync();
    anotherTask.FireAndForget(
        onException: ex => LogError(ex) // Log error if exception occurs
    );
    ```

#### Other Utilities
Other utilities provide additional functionalities and helpers to extend existing concepts, making them more convenient and robust for everyday use.

- **Ensure.cs**: Offers a variety of methods to ensure arguments meet certain conditions, throwing exceptions if they do not. This utility is very useful for validating method inputs and maintaining contracts within the code.
You can add the `Ensure` class as a static using to simplify the call. 

##### Examples:
1. **Using Ensure to validate arguments:**
    ```csharp
    public void ProcessData(string input) {
        input = Ensure.IsNotNullOrWhiteSpace(input);
        // Rest of the method logic
    }
    ```

2. **Using Ensure as a static using:**
    ```csharp
    using static DotNetToolbox.Ensure;
    
    // class dedinition and other code
    
    public void ProcessList(IList<string> items) {
        items = HasNoNullOrWhiteSpace(items);

        // Rest of the method logic
    }
    ```

- **CreateInstance.cs**: Provides methods to dynamically create instances of types, which can be very helpful for creating types with non-public constructors or when types need to be created based on runtime data.

##### Examples:
1. **Creating an instance of a class with private constructor:**
    ```csharp
    var instance1 = CreateInstance.Of<SomeClassWithPrivateConstructor>();
    var instance2 = CreateInstance.Of<SomeClassWithArguments>(arg1, arg2);
    ```

2. **Creating an instance using dependency injection:**
    ```csharp
    public class SomeClassWithArgumentsAndServices {
        public SomeClassWithArgumentsAndServices(string arg1, string arg2, ILoggerFactory loggerFactory) {
            // ...
        }
    }
    
    // ...
    
    var serviceProvider = // ... build your service provider
    var instance = CreateInstance.Of<SomeClassWithArgumentsAndServices>(serviceProvider, arg1, arg2); // the injected services do not need to be added.
    ```

- **UrlSafeBase64String**: A record struct that represents a base64 encoded string safe for URL use. It can be used to encode and decode bytes, strings, and GUIDs to a URL-safe base64 format.

##### Examples:
1. **Encoding and decoding a URL-safe base64 string:**
    ```csharp
    byte[] original = { 0x1, 0x2, 0x3 };
    UrlSafeBase64String encoded = original;
    byte[] decoded = encoded;

    encoded.Should().BeOfType<string>(); // Outputs URL-safe base64 string
    decoded.Should().BeEquivalentTo(original); // Outputs True
    ```

2. **Working with GUIDs and base64 strings:**
    ```csharp
    Guid original = Guid.NewGuid();
    UrlSafeBase64String encoded = original;
    Guid decoded = encoded;

    encoded.Should().BeOfType<string>(); // Outputs URL-safe base64 string
    decoded.Should().Be(original); // Outputs True
    ```

- **IValidatable**: An interface that classes can implement to provide custom validation logic, returning a `Result` that indicates whether the validation was successful or not.

##### Examples:
1. **Implementing IValidatable in a class:**
   ```csharp
   using static DotNetToolbox.Result;

   public class Person : IValidatable {
       public string? Name { get; set; }
       public int Age { get; set; }

       public Result Validate(IDictionary<string, object?>? context = null) {
           var result = Success();
           if (string.IsNullOrWhiteSpace(Name)) {
               result += InvalidData(nameof(Name), "Name cannot be empty or whitespace.");
           }

           if (Age < 0 || Age > 120) {
               result += InvalidData(nameof(Age), "Age must be between 0 and 120.");
           }

           return result; // may return multiple errors
       }
   }
   ```

2. **Using Ensure to validate an IValidatable object:**
   ```csharp
   using static DotNetToolbox.Ensure;

   public void ProcessPerson(Person person) {
       person = IsValid(person); // throws ValidationException if validation fails.

       // Rest of the method logic
   }
   ```

#### Installation
```bash
Install-Package DotNetToolbox.Core -Version 8.0.2-rc2
```

#### Dependencies
- .NET 8
