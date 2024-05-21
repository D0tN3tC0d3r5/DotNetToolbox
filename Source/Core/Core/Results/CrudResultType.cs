namespace DotNetToolbox.Results;

public enum CrudResultType : byte {
    Invalid = 0, // The request validation failed.
    NotFound = 1, // The requested resource was not found.
    Conflict = 2, // A conflict has occured blocking the operation.
    Success = 3, // The operation was successful.
    Error = 255, // An internal error has occurred.
}
