### DotNetToolbox.Core README

#### Introduction
DotNetToolbox.Core is a versatile C# library for .NET 8, designed to enhance development by providing a range of utilities and patterns. It simplifies complex tasks and improves code testability.

#### Table of Contents
1. [Result Patterns](#result-patterns)
2. [System Utilities](#system-utilities)
3. [Key Utilities](#key-utilities)
4. [Extension Classes](#extension-classes)
5. [Usage Examples](#usage-examples)
6. [Installation](#installation)
7. [Dependencies](#dependencies)
8. [Contributing](#contributing)
9. [License](#license)

#### Result Patterns
- **Result.cs**: Core class for generic operation results. Used mostly for validation and error handling.

The `Result` class provides a flexible way to represent the outcome of operations, whether successful, invalid due to data issues, or errored due to exceptions. Below are some examples of how to use the `Result` class:

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

6. **Returning a Value on Successful Operation:**
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

7. **Handling an Error with `Result<TValue>`:**
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

8. **Indicating a Successful CRUD Operation:**
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

9. **Handling a 'Not Found' Scenario in CRUD Operations:**
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

10. **Returning a Value on Successful 'Read' Operation:**
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

11. **Handling a 'Conflict' in CRUD Operations:**
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

12. **Returning an 'Ok' HTTP Response:**
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

13. **Handling a 'Bad Request' Scenario:**
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

14. **Returning Data on Successful Operation:**
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

15. **Handling an Unauthorized Access:**
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

16. **Successful Sign-In:**
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

17. **Handling Locked Account:**
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

18. **Two-Factor Authentication Required:**
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
- **DateTimeProvider.cs**, **GuidProvider.cs**: Facilitate dependency injection and testing.
- **FileSystem.cs**: Abstraction for file system operations.
- **Input.cs**, **Output.cs**: Utilities for input and output operations.

#### Key Utilities
- **CreateInstance.cs**: Dynamic object instantiation.
- **Ensure.cs**: Argument validation utility.

#### Extension Classes
- **EnumerableExtensions.cs**: Extends enumerable collections.
- **ConcurrentDictionaryExtensions.cs**: Enhances `ConcurrentDictionary`.
- **TaskExtensions.cs**: Adds methods to `Task` and `ValueTask`.

#### Usage Examples
(Specific code snippets demonstrating the usage of key features.)

#### Installation
```bash
Install-Package DotNetToolbox.Core -Version 8.0.2-rc1
```

#### Dependencies
- .NET 8

#### Contributing
(Guidelines for contributing to the project.)

#### License
(Details of the project's license.)
