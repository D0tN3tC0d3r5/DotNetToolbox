namespace System.Results;

public enum CrudResultType {
    ValidationFailure = 0,
    Success = 1,
    NotFound = 2,
    Conflict = 3,
}
