namespace DotNetToolbox.Results;

public enum ResultType : byte {
    Invalid = 0, // The request validation failed.
    Success = 1, // The operation was successful.
    Error = 255, // An exception has occurred.
}
